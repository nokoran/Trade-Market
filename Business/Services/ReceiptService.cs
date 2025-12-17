using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        
        public ReceiptService(IUnitOfWork context, IMapper mapper)
        {
            _uow = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ReceiptModel>>(await _uow.ReceiptRepository.GetAllWithDetailsAsync());
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            return _mapper.Map<ReceiptModel>(await _uow.ReceiptRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task AddAsync(ReceiptModel model)
        {
            await _uow.ReceiptRepository.AddAsync(_mapper.Map<Receipt>(model));
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            _uow.ReceiptRepository.Update( _mapper.Map<Receipt>(model));
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await _uow.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            foreach (var a in receipt.ReceiptDetails)
            {
                _uow.ReceiptDetailRepository.Delete(a);
            }
            await _uow.ReceiptRepository.DeleteByIdAsync(modelId);
            await _uow.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _uow.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            
            if (receipt == null)
            {
                throw new MarketException("Receipt not found");
            }
            

            if (receipt.ReceiptDetails == null || !receipt.ReceiptDetails.Select(x => x.ProductId).Contains(productId))
            {
                var product = await _uow.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new MarketException("Product not found");
                }
                ReceiptDetail newReceiptDetail = new ReceiptDetail
                {
                    ReceiptId = receiptId,
                    ProductId = productId,
                    Quantity = quantity,
                    DiscountUnitPrice = product.Price * (1 - (decimal)receipt.Customer.DiscountValue / 100m),
                    UnitPrice = product.Price,
                    Product = product,
                    Receipt = receipt
                };
                await _uow.ReceiptDetailRepository.AddAsync(newReceiptDetail);
                
            }
            else
            {
                receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity += quantity;
            }
            _uow.ReceiptRepository.Update(receipt);
            await _uow.SaveAsync();
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _uow.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity -= quantity;
            if (receipt.ReceiptDetails.First(x => x.ProductId == productId).Quantity <= 0)
            {
                _uow.ReceiptDetailRepository.Delete(receipt.ReceiptDetails.First(x => x.ProductId == productId));
            }
            _uow.ReceiptRepository.Update(receipt);
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            return _mapper.Map<IEnumerable<ReceiptDetailModel>>((await _uow.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).ReceiptDetails);
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            return (await _uow.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).ReceiptDetails.
                Aggregate(0m, (current, receiptDetail) => current + receiptDetail.DiscountUnitPrice * receiptDetail.Quantity);
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _uow.ReceiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            _uow.ReceiptRepository.Update(receipt);
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await _uow.ReceiptRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ReceiptModel>>(receipts.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate));
        }
    }
}
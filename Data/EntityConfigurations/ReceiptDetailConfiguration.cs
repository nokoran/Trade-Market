using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations
{
    public class ReceiptDetailConfiguration : IEntityTypeConfiguration<ReceiptDetail>
    {
        public void Configure(EntityTypeBuilder<ReceiptDetail> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasKey(x => new { x.ReceiptId, x.ProductId });
        }
    }
}
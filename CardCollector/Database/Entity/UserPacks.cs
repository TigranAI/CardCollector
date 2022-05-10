using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace CardCollector.Database.Entity
{
    public class UserPacks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual User User { get; set; }
        public virtual Pack Pack { get; set; }
        public int Count { get; set; }

        public UserPacks(User user, Pack pack, int count = 0)
        {
            User = user;
            Pack = pack;
            Count = count;
        }

        public UserPacks()
        {
        }

        public async Task<Sticker> Open()
        {
            if (Count <= 0) throw new AggregateException("Packs count must be greater than zero!");
            Count--;
            Pack.OpenedCount++;
            return Pack.IsExclusive
                ? Pack.OpenExclusive(User.Stickers)
                : await Pack.Open();
        }
    }
}
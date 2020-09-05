using System;
namespace TamagotchiAPI.Models
{
    public class Playtime
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public PetId petId { get; set; }
    }
}
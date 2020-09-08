using System;
namespace TamagotchiAPI.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int HungerLevel { get; set; }
        public int HappinessLevel { get; set; }
    }
}
namespace Dojodachi.Models
{
    public class Character
    {
        public string Name {get;set;}
        public int Fullness {get;set;}
        public int Happiness {get;set;}
        public int Energy {get;set;}
        public int Meals {get;set;}
        public Character()
        {
            this.Name = "Dojodachi";
            this.Fullness = 20;
            this.Happiness = 20;
            this.Energy = 50;
            this.Meals = 3;
        }
    }
}
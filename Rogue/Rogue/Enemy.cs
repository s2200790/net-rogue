namespace Rogue
{
    public class Enemy
    {
        public string Name { get; set; }
        public int SpriteId { get; set; }
        public int HitPoints { get; set; }

        public Enemy() { }

        public Enemy(string name, int spriteId, int hitPoints)
        {
            Name = name;
            SpriteId = spriteId;
            HitPoints = hitPoints;
        }

        public Enemy(Enemy copyFrom)
        {
            Name = copyFrom.Name;
            SpriteId = copyFrom.SpriteId;
            HitPoints = copyFrom.HitPoints;
        }

        public override string ToString()
        {
            return $"Enemy: {Name}, SpriteId: {SpriteId}, HP: {HitPoints}";
        }
    }
}
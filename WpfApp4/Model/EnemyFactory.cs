using System;

namespace Model
{
    public static class EnemyFactory
    {
        public static Enemy CreateBaseGoblin()
        {
            return new Enemy 
            { 
                Name = "Гоблин",
                Race = EnemyRace.Goblin, 
                HP = 30, 
                Attack = 12, 
                Defense = 3, 
                CritChance = 0.20 
            };
        }

        public static Enemy CreateBaseSkeleton()
        {
            return new Enemy 
            { 
                Name = "Скелет", 
                Race = EnemyRace.Skeleton, 
                HP = 40, 
                Attack = 10, 
                Defense = 5, 
                IgnorePlayerDefense = true 
            };
        }

        public static Enemy CreateBaseMage()
        {
            return new Enemy 
            { Name = "Маг", 
                Race = EnemyRace.Mage, 
                HP = 25, 
                Attack = 15, 
                Defense = 2, 
                FreezeChance = 0.15 
            };
        }

        public static Enemy CreateBaseSlime()
        {
            return new Enemy 
            { 
                Name = "Слизень", 
                Race = EnemyRace.Slime, 
                HP = 35, 
                Attack = 6, 
                Defense = 1, 
                ReduceIncomingDamage = true, 
                DamageReduction = 2 
            };
        }

        public static Enemy CreateBoss(int bossIndex)
        {
            Enemy e;
            switch (bossIndex)
            {
                case 0: e = CreateBaseGoblin(); break;
                case 1: e = CreateBaseSkeleton(); break;
                case 2: e = CreateBaseMage(); break;
                case 3: e = CreateBaseSkeleton(); break;
                default: e = CreateBaseGoblin(); break;
            }

            switch (bossIndex)
            {
                case 0:
                    e.Name = "ВВГ";
                    e.HP = (int)(e.HP * 2.0);
                    e.Attack = (int)Math.Ceiling(e.Attack * 1.5);
                    e.Defense = (int)Math.Ceiling(e.Defense * 1.2);
                    e.CritChance += 0.10;
                    break;
                case 1:
                    e.Name = "Ковальский";
                    e.HP = (int)(e.HP * 2.5);
                    e.Attack = (int)Math.Ceiling(e.Attack * 1.3);
                    e.Defense = (int)Math.Ceiling(e.Defense * 1.4);
                    e.IgnorePlayerDefense = true;
                    break;
                case 2:
                    e.Name = "Архимаг C++";
                    e.HP = (int)(e.HP * 1.8);
                    e.Attack = (int)Math.Ceiling(e.Attack * 1.6);
                    e.Defense = (int)Math.Ceiling(e.Defense * 1.1);
                    e.FreezeChance += 0.10;
                    break;
                case 3:
                    e.Name = "Пестов S--";
                    e.HP = (int)(e.HP * 1.3);
                    e.Attack = (int)Math.Ceiling(e.Attack * 1.8);
                    e.Defense = Math.Max(0, (int)Math.Floor(e.Defense * 0.6));
                    e.IgnorePlayerDefense = true;
                    e.FreezeChance = 0.18 + 0.15;
                    break;
            }
            return e;
        }

        public static Enemy GenerateRandomEnemy()
        {
            int t = RandomChoice.Next(4);
            switch (t)
            {
                case 0: return CreateBaseGoblin();
                case 1: return CreateBaseSkeleton();
                case 2: return CreateBaseMage();
                case 3: return CreateBaseSlime();
                default: return CreateBaseGoblin();
            }
        }
    }
}
using Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp4;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private Player player;
        private Enemy currentEnemy;
        private object pendingItem = null;
        private int turn = 0;

        public MainWindow()
        {
            InitializeComponent();
            player = new Player();
            NextTurn();
            UpdateUI();
        }

        private void NextTurn()
        {
            turn++;
            FloorText.Text = $"Этаж: {turn}";
            CombatPanel.Visibility = Visibility.Collapsed;
            ItemPanel.Visibility = Visibility.Collapsed;
            pendingItem = null;

            Log($"Ход {turn}");

            if (turn % 10 == 0)
            {
                currentEnemy = EnemyFactory.CreateBoss(RandomChoice.Next(4));
                RoomText.Text = $"БОСС: {currentEnemy.Name}\nHP: {currentEnemy.HP}";
                Log("Появился БОСС!");
                CombatPanel.Visibility = Visibility.Visible;
            }
            else if (RandomChoice.Next(2) == 0)
            {
                currentEnemy = null;
                RoomText.Text = "Сундук";
                Log("Вы нашли сундук!");

                pendingItem = ItemGenerator.GenerateRandomItem();

                if (pendingItem is Weapon w)
                {
                    RoomText.Text += $"\nНовое оружие: {w}";
                    Log($"Новое оружие: {w}");
                    Log($"Текущее: {player.Weapon}");
                }
                else if (pendingItem is Armor a)
                {
                    RoomText.Text += $"\nНовый доспех: {a}";
                    Log($"Новый доспех: {a}");
                    Log($"Текущий: {player.Armor}");
                }

                if (pendingItem is string s && s == "potion")
                {
                    player.HealFull();
                    Log("Вы полностью исцелились!");
                    RoomText.Text = "Сундук\nЛечебное зелье: +HP";
                    UpdateRoomImage();  
                    NextTurn();         
                    return;
                }

                ItemPanel.Visibility = Visibility.Visible;
            }
            else
            {
                currentEnemy = EnemyFactory.GenerateRandomEnemy();
                RoomText.Text = $"Враг: {currentEnemy.Name}\nHP: {currentEnemy.HP}";
                Log($"Появился {currentEnemy.Name}");
                CombatPanel.Visibility = Visibility.Visible;
            }

            UpdateUI();
            UpdateRoomImage();
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null) return;

            int damage = 8 + player.Weapon.Attack - currentEnemy.Defense;
            damage = System.Math.Max(1, damage);

            currentEnemy.HP -= damage;
            Log($"Вы нанесли {damage} урона");

            EnemyTurn();
        }

        private void Defend_Click(object sender, RoutedEventArgs e)
        {
            if (currentEnemy == null) return;

            player.Defending = true;
            Log("Вы в защите");

            EnemyTurn();
        }

        private void EnemyTurn()
        {
            if (currentEnemy != null && currentEnemy.HP > 0)
            {
                Enemy.ProcessEnemyAttack(player, currentEnemy);
            }

            if (currentEnemy != null && currentEnemy.HP <= 0)
            {
                Log("Враг побежден!");
                currentEnemy = null;
                NextTurn();
            }

            if (player.HP <= 0)
            {
                MainFrame.Navigate(new DeadWindow());
            }

            UpdateUI();
            UpdateEnemyUI();
        }

        private void UpdateUI()
        {
            HPText.Text = $"HP: {player.HP}";
            WeaponText.Text = $"Оружие: {player.Weapon}";
            ArmorText.Text = $"Броня: {player.Armor}";
        }

        private void UpdateEnemyUI()
        {
            if (currentEnemy != null)
            {
                RoomText.Text = $"Враг: {currentEnemy.Name}\nHP: {currentEnemy.HP}";
            }
            UpdateRoomImage();
        }

        private void Log(string text)
        {
            TextBlock tb = new TextBlock { Text = text };
            LogPanel.Children.Add(tb);
            LogScroll.ScrollToEnd();
        }

        private void TakeItem_Click(object sender, RoutedEventArgs e)
        {
            if (pendingItem is Weapon w)
            {
                player.Weapon = w;
                Log("Оружие заменено");
            }
            else if (pendingItem is Armor a)
            {
                player.Armor = a;
                Log("Доспех заменён");
            }

            pendingItem = null;
            ItemPanel.Visibility = Visibility.Collapsed;
            NextTurn();
        }

        private void DropItem_Click(object sender, RoutedEventArgs e)
        {
            Log("Предмет выброшен");
            pendingItem = null;
            ItemPanel.Visibility = Visibility.Collapsed;
            NextTurn();
        }

        private void UpdateRoomImage()
        {
            string imgPath = "";

            if (currentEnemy != null)
            {
                switch (currentEnemy.Race)
                {
                    case EnemyRace.Goblin: imgPath = "images/goblin.png"; break;
                    case EnemyRace.Skeleton: imgPath = "images/skeleton.png"; break;
                    case EnemyRace.Mage: imgPath = "images/mage.png"; break;
                    case EnemyRace.Slime: imgPath = "images/slime.png"; break;
                }
            }
            else if (pendingItem != null)
            {
                if (pendingItem is Weapon || pendingItem is Armor)
                    imgPath = "images/chest.png";
                else if (pendingItem is string s && s == "potion")
                    imgPath = "images/potion.png";
            }
            else
            {
                RoomImage.Source = null;
                return;
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new System.Uri($"pack://application:,,,/{imgPath}", System.UriKind.Absolute);
            bitmap.EndInit();
            RoomImage.Source = bitmap;
        }
    }
}
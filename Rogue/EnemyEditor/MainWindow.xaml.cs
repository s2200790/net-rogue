using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Rogue;

namespace EnemyEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddEnemyToList(object sender, RoutedEventArgs e)
        {
            string name = EnemyNameEntry.Text;
            string spriteIdText = EnemySpriteIdEntry.Text;
            string hitPointsText = EnemyHitpointsEntry.Text;

            if (int.TryParse(spriteIdText, out int spriteId) && spriteId >= 0 &&
                int.TryParse(hitPointsText, out int hitPoints) && hitPoints > 0 &&
                !string.IsNullOrEmpty(name))
            {
                Enemy newEnemy = new Enemy(name, spriteId, hitPoints);
                EnemyList.Items.Add(newEnemy);
                ErrorLabel.Text = "Enemy added!";
            }
            else
            {
                ErrorLabel.Text = "Invalid input! Check all fields.";
            }
        }

        private void SaveEnemiesToJSON(object sender, RoutedEventArgs e)
        {
            List<Enemy> enemies = new List<Enemy>();
            foreach (var item in EnemyList.Items)
            {
                if (item is Enemy enemy)
                {
                    enemies.Add(enemy);
                }
            }

            string json = JsonConvert.SerializeObject(enemies, Formatting.Indented);
            string filename = "enemies.json";

            try
            {
                File.WriteAllText(filename, json);
                ErrorLabel.Text = "Enemies saved to JSON!";
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = $"Error saving file: {ex.Message}";
            }
        }

        private void EnemyList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EnemyList.SelectedItem is Enemy selectedEnemy)
            {
                EnemyNameEntry.Text = selectedEnemy.Name;
                EnemySpriteIdEntry.Text = selectedEnemy.SpriteId.ToString();
                EnemyHitpointsEntry.Text = selectedEnemy.HitPoints.ToString();
            }
        }
    }
}
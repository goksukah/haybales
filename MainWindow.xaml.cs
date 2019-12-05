using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Net.Http;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace Plantville_Try3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Seed> seedInventory = new List<Seed>() {
                                   new Strawberry(),
                                   new Pear(),
                                   new Spinach()
        };
        List<Plant> garden = new List<Plant>();
        List<Plant> inventory = new List<Plant>();
        List<Model> ModelList = new List<Model>();

        Farmer farmer = new Farmer("user1");



        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            garden_Btn_Click(new object(), new RoutedEventArgs()); 
        }

        public void SaveData()
        {
            Template template = new Template();


            template.Garden = garden;
            template.Inventory = inventory;
            template.Money = farmer.Money;
            template.Land = farmer.Land;

            string json = JsonConvert.SerializeObject(template);

            //write string to file

            //Current Directory
            String path = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(path).Parent.Parent.FullName;
            string game_Data = projectDirectory + "\\game_Data.txt";

            System.IO.File.WriteAllText(game_Data, json);

        }


        private void garden_Btn_Click(object sender, RoutedEventArgs e)
        {
            money_lbl.Content = farmer.Money;
            land_lbl.Content = farmer.Land;
            seeds_Btn.IsEnabled = true;
            username_display_Lbl.Content = farmer.Name;

            garden_Grid.Visibility = System.Windows.Visibility.Visible;
            inventory_Grid.Visibility = System.Windows.Visibility.Hidden;
            seeds_Grid.Visibility = System.Windows.Visibility.Hidden;
            chat_Grid.Visibility = System.Windows.Visibility.Hidden;
            trade_Grid.Visibility = System.Windows.Visibility.Hidden;
            propose_trade_Grid.Visibility = Visibility.Hidden;



            if (garden.Count > 0)
            {
                harvest_all_Btn.IsEnabled = true;

                //dont add item if it is already added
                garden_ListBox.Items.Clear();
                foreach (Plant item in garden)
                {
                    garden_ListBox.Items.Add(item.TimeLeft(item.PlantedTime, item.HarvestDuration));
                }

            }

            else
            {
                harvest_all_Btn.IsEnabled = false;
            }
        }

        private void seeds_Btn_Click(object sender, RoutedEventArgs e)
        {
            garden_Grid.Visibility = System.Windows.Visibility.Hidden;
            inventory_Grid.Visibility = System.Windows.Visibility.Hidden;
            seeds_Grid.Visibility = System.Windows.Visibility.Visible;
            chat_Grid.Visibility = Visibility.Hidden;
            trade_Grid.Visibility = Visibility.Hidden;
            propose_trade_Grid.Visibility = Visibility.Hidden;

            seeds_ListBox.Items.Clear();
            for (int i = 0; i < seedInventory.Count; i++)
            {
                seeds_ListBox.Items.Add(seedInventory[i]);

            }

        }


        private void inventory_Btn_Click(object sender, RoutedEventArgs e)
        {
            garden_Grid.Visibility = System.Windows.Visibility.Hidden;
            inventory_Grid.Visibility = System.Windows.Visibility.Visible;
            seeds_Grid.Visibility = System.Windows.Visibility.Hidden;
            chat_Grid.Visibility = System.Windows.Visibility.Hidden;
            trade_Grid.Visibility = System.Windows.Visibility.Hidden;
            propose_trade_Grid.Visibility = Visibility.Hidden;


            inv_ListBox.Items.Clear();

            int stwCounter=0;
            int pearCounter = 0;
            int spnhCounter = 0;

            var numberGroups = inventory.GroupBy(i => i);
            foreach (var g in numberGroups)
            {

                if (g.Key.Seed.Name == "strawberry")
                {
                    stwCounter++;
                }
                if (g.Key.Seed.Name == "pear")
                {
                    pearCounter++;
                }
                if (g.Key.Seed.Name == "spinach")
                {
                    spnhCounter++;
                }
            }
            if (stwCounter > 0)
                {
                    inv_ListBox.Items.Add("strawberry " + "[" + stwCounter + "]");
                }
                if (pearCounter > 0)
                {
                    inv_ListBox.Items.Add("pear " + "[" + pearCounter + "]");
                }
                if (spnhCounter > 0)
                {
                    inv_ListBox.Items.Add("spinach " + "[" + spnhCounter + "]");
                }
 
        }

        private void inv_sale_Btn_Click(object sender, RoutedEventArgs e)
        {
            int inventory_count = inventory.Count;
            int earned_money = 0;
            if (inventory.Count > 0)
            {

                for (int i = 0; i < inventory_count; i++)
                {
                    earned_money = earned_money + inventory[i].Seed.HarvestPrice;

                }
                earned_money = earned_money - 10;
                farmer.Money = farmer.Money + earned_money; 
                money_lbl.Content = farmer.Money;

                MessageBox.Show("You earned " + earned_money + " from this trade");
                inventory.Clear();
                inv_ListBox.Items.Clear();

            }
            else
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to go to the farmer`s market without any items in your inventory? Yes/No", "Delete", MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    earned_money = earned_money - 10;
                    farmer.Money = farmer.Money + earned_money; 
                    money_lbl.Content = farmer.Money;
                }
                if (dialogResult == MessageBoxResult.No)
                {
                    /// do nothing here        
                }
            }
        }

        private void seeds_ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = seeds_ListBox.SelectedIndex;
            var selectedSeed = seedInventory[index];

            if (!(seeds_ListBox.SelectedIndex == -1) && farmer.Money > 0 && farmer.Land > 0)
            {
                farmer.Money = farmer.Money - selectedSeed.SeedPrice;
                money_lbl.Content = farmer.Money;
                land_lbl.Content = --farmer.Land;
                garden.Add(new Plant(selectedSeed));
                MessageBox.Show("You purchased a " + selectedSeed.Name);

            }
            else
            {
                MessageBox.Show("Please check your money and land status");
            }
        }


        private void LoadData()
        {
            //Current Directory
            String path = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(path).Parent.Parent.FullName;
            string game_Data = projectDirectory + "\\game_Data.txt";

            if (File.Exists(game_Data))
            {
                using (var sr = new StreamReader(game_Data))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
                        garden = JsonConvert.DeserializeObject<List<Plant>>(data["Garden"].ToString());
                        inventory = JsonConvert.DeserializeObject<List<Plant>>(data["Inventory"].ToString());
                        farmer.Money = Convert.ToInt32(data["Money"]);
                        farmer.Land = Convert.ToInt32(data["Land"]);
                        land_lbl.Content = farmer.Land.ToString();
                        money_lbl.Content = farmer.Money.ToString();
                    }


                }



            }



        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveData();
        }

        private void harvest_all_Btn_Click(object sender, RoutedEventArgs e)
        {
            int garden_count = garden.Count;
            if (garden.Count > 0)
            {

                for (int i = garden_count - 1; i >= 0; i--)
                {

                    if (garden[i].IsRipe() && !(garden[i].IsSpoiled())) //isGoodtoHarvest garden[i].PlantedTime, garden[i].HarvestDuration))
                    {
                        inventory.Add(garden[i]);
                        garden.RemoveAt(i);
                        garden_ListBox.Items.RemoveAt(i);
                        farmer.Land++;

                    }
                    else if (!(garden[i].IsRipe()))//not ripe meaning also not spoiled
                    {
                        //do nothing

                    }

                    else
                    {
                       //SPOILED
                        garden.RemoveAt(i);
                        garden_ListBox.Items.RemoveAt(i);

                        farmer.Land++;

                    }
                }

                MessageBox.Show("All available items harvested!");
            }

            else
            {
                harvest_all_Btn.IsEnabled = false;
            }

        }


        private void garden_ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (garden_ListBox.Items.Count > 0 && garden_ListBox.SelectedIndex > -1)
            {


                int i = garden_ListBox.SelectedIndex;
                if (garden[i].IsRipe() && !(garden[i].IsSpoiled())) //isGoodtoHarvest garden[i].PlantedTime, garden[i].HarvestDuration))
                {
                    inventory.Add(garden[i]);
                    garden.RemoveAt(i);
                    garden_ListBox.Items.RemoveAt(i);
                    farmer.Land++;

                }
                else if (!(garden[i].IsRipe()))//not ripe meaning also not spoiled
                {
                    //do nothing

                }

                else
                {
                    //SPOILED
                    garden.RemoveAt(i);
                    garden_ListBox.Items.RemoveAt(i);

                    farmer.Land++;

                }

                land_lbl.Content = farmer.Land;

                MessageBox.Show(garden[i].Seed.Name + " harvested");
            }
        }

        private void chat_Btn_Click(object sender, RoutedEventArgs e)
        {
            ModelList.Clear();

            garden_Grid.Visibility = Visibility.Hidden;
            inventory_Grid.Visibility = Visibility.Hidden;
            seeds_Grid.Visibility = Visibility.Hidden;
            chat_Grid.Visibility = Visibility.Visible;
            trade_Grid.Visibility = Visibility.Hidden;
            propose_trade_Grid.Visibility = Visibility.Hidden;


            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://plantville.herokuapp.com/");

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }


            var response = JsonConvert.DeserializeObject<dynamic>(jsonString);

            foreach (var data in response)
            {
                Model model = new Model();
                model.Modeltype = data.model;
                model.Pk = data.pk;


                UserMessage userMessage = new UserMessage();
                var innerdata = data.fields;

                userMessage.Username = innerdata.username;
                userMessage.Message = innerdata.message;

                model.UserMessage = userMessage;

                ModelList.Add(model);


            }

            chatroom_ListBox.Items.Clear();

            foreach (var chat in ModelList)
            {

                chatroom_ListBox.Items.Add(string.Format("{0}: {1}", chat.UserMessage.Username, chat.UserMessage.Message));

            }

        }

        private void login_Btn_Click(object sender, RoutedEventArgs e)
        {
            string name = username_txtBox.Text.ToString();
            var regex = "^[a-zA-Z0-9]+$";
            var match = Regex.Match(name, regex, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                farmer.Name = name;
                username_display_Lbl.Content = farmer.Name;
                signin_Grid.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Please enter a proper value for the username field. \n -Letters or numbers");

            }


        }



        private void username_txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login_Btn_Click(new object(), new RoutedEventArgs());
            }
        }

        private async void chat_send_Btn_ClickAsync(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();

            var data = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("username", farmer.Name),
                    new KeyValuePair<string, string>("message", chatroom_TextBox.Text.ToString())
                });

            HttpResponseMessage response = await client.PostAsync("http://plantville.herokuapp.com/", data);

            string json_data = await response.Content.ReadAsStringAsync();
            chat_Btn_Click(new object(), new RoutedEventArgs());
            chatroom_TextBox.Text = "";
        }

        private void chatroom_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                chat_send_Btn_Click(new object(), new RoutedEventArgs());
            }
        }
        private void chat_send_Btn_Click(object sender, RoutedEventArgs e)
        {
            chat_send_Btn_ClickAsync(new object(), new RoutedEventArgs());
        }

        private void trade_menuBtn_Click(object sender, RoutedEventArgs e)
        {
            trade_Grid.Visibility = System.Windows.Visibility.Visible;
            garden_Grid.Visibility = System.Windows.Visibility.Hidden;
            chat_Grid.Visibility = System.Windows.Visibility.Hidden;
            inventory_Grid.Visibility = Visibility.Hidden;
            propose_trade_Grid.Visibility = Visibility.Hidden;
            seeds_Grid.Visibility = Visibility.Hidden;


            ModelList.Clear();



            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://plantville.herokuapp.com/trades");

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            var response = JsonConvert.DeserializeObject<dynamic>(jsonString);

            foreach (var data in response)
            {
                Model model = new Model();
                model.Modeltype = data.model;
                model.Pk = data.pk;


                TradeTemplate tradeTemplate = new TradeTemplate();
                var innerdata = data.fields;

                tradeTemplate.Author = innerdata.author;
                tradeTemplate.Price = innerdata.price;
                tradeTemplate.State = innerdata.state;
                tradeTemplate.Plant = innerdata.plant;
                tradeTemplate.Quantity = innerdata.quantity;
                if (innerdata.accepted_by != null)
                {
                    tradeTemplate.AcceptedBy = innerdata.accepted_by;
                }

                model.TradeTemplate = tradeTemplate;

                ModelList.Add(model);


            }

            trade_ListBox.Items.Clear();
            foreach (var trade in ModelList)
            {
                if (trade.TradeTemplate.State == "open")
                {
                    ListBoxItem listBoxItem = new ListBoxItem();
                    listBoxItem.Content = "[" + trade.TradeTemplate.State + "]: " + trade.TradeTemplate.Author
                        + " wants to buy " + trade.TradeTemplate.Quantity + " " + trade.TradeTemplate.Plant + " for $" + trade.TradeTemplate.Price + " .";
                    listBoxItem.DataContext = trade.Pk;
                    trade_ListBox.Items.Add(listBoxItem);
                }
                else //closed
                {
                    trade_ListBox.Items.Add(string.Format("[{0}]: {1} bought {2} {3} for ${4} from {5}.", trade.TradeTemplate.State, trade.TradeTemplate.Author, trade.TradeTemplate.Quantity, trade.TradeTemplate.Plant, trade.TradeTemplate.Price, trade.TradeTemplate.AcceptedBy));

                }
            }


        }

        private void propose_trade_MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            garden_Grid.Visibility = Visibility.Hidden;
            chat_Grid.Visibility = Visibility.Hidden;
            inventory_Grid.Visibility = Visibility.Hidden;
            seeds_Grid.Visibility = Visibility.Hidden;
            trade_Grid.Visibility = Visibility.Hidden;
            propose_trade_Grid.Visibility = Visibility.Visible;

            foreach (Seed p in seedInventory)
            {
                plant_comboBox.Items.Add(p);

            }

        }

        private async void prop_submit_Btn_ClickAsync(object sender, RoutedEventArgs e)
        {

            var client = new HttpClient();

            var data = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("author", farmer.Name),
                    new KeyValuePair<string, string>("state", "open"),
                    new KeyValuePair<string, string>("quantity", quantity_TextBox.Text.ToString()),
                    new KeyValuePair<string, string>("price",  price_TxtBox.Text.ToString()),
                    new KeyValuePair<string, string>("plant",  plant_comboBox.SelectedItem.ToString().TrimStart(','))



                });

            HttpResponseMessage response = await client.PostAsync("http://plantville.herokuapp.com/trades", data);

            string json_data = await response.Content.ReadAsStringAsync();
            quantity_TextBox.Text = "";
            price_TxtBox.Text = "";
            plant_comboBox.SelectedValue = -1;

        }
        private void prop_submit_Btn_Click(object sender, RoutedEventArgs e)
        {
            prop_submit_Btn_ClickAsync(new object(), new RoutedEventArgs());
        }

        private async void accept_trade_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (trade_ListBox.SelectedItem != null)
            {
                ListBoxItem lbi = (trade_ListBox.SelectedItem as ListBoxItem);


                if (!string.IsNullOrEmpty(lbi.DataContext.ToString()))
                {
                    string pk = lbi.DataContext.ToString();

                    var client = new HttpClient();

                    var data = new FormUrlEncodedContent(new[]
                    {

                        new KeyValuePair<string, string>("trade_id", pk),
                        new KeyValuePair<string, string>("accepted_by", farmer.Name)


                    });


                    HttpResponseMessage response = await client.PostAsync("http://plantville.herokuapp.com/accept_trade", data);


                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var model in ModelList)
                        {
                            if (model.Pk.ToString() == pk)
                            {
                                int quant = model.TradeTemplate.Quantity;
                                string plant = model.TradeTemplate.Plant;
                                int price = model.TradeTemplate.Price;

                                farmer.Money = farmer.Money + price;
                                money_lbl.Content = farmer.Money;

                                for (int i = quant-1; i >= 0; i--)
                                {
                                    if (inventory[i].Seed.Name == plant)
                                    {
                                        inventory.Remove(inventory[i]);
                                    }

                                }
                                

                            }

                            else
                            {  //empty}
                            }

                        }
                        MessageBox.Show("Nice catch! Keep trading with your friends!");

                    }
                    else
                    {
                        MessageBox.Show("Please check your inventory and money in order to complete the trade.");
                    }


                    trade_menuBtn_Click(new object(), new RoutedEventArgs());

                }
                else
                {
                    MessageBox.Show("Please select an open trade.");
                }

            }
            else
            {
                MessageBox.Show("Something went wrong...Sorry, it`s on us.");
            }

        }

       
    }
}

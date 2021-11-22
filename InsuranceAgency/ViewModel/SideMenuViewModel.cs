using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InsuranceAgency.ViewModel
{
    class SideMenuViewModel
    {
        //to call resource dictionary in our code behind
        private ResourceDictionary dict = Application.LoadComponent(new Uri("/InsuranceAgency;component/Assets/IconDictionary.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;


        //Our Source List for Menu Items
        public List<MenuItemsData> MenuList
        {
            get
            {
                if (Database.Admin)
                {
                    return new List<MenuItemsData>
                    {
                        //MainMenu without SubMenu Button 
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_policy"], MenuText="Policy", MenuName = "Полис", SubMenuList=null },

                        //MainMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_policyholder"], MenuText="AddPolicyholder", MenuName = "Страхователи"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="AddPolicyholder", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_change"], SubMenuText="СhangePolicyholder", SubMenuName = "Изменить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="AllPolicyholders", SubMenuName = "Все" }}
                        },

                        //MainMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_people"], MenuText="AddPersonAllowedToDrive", MenuName = "Водители"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="AddPersonAllowedToDrive", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_change"], SubMenuText="СhangePersonAllowedToDrive", SubMenuName = "Изменить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="AllPersonsAllowedToDrive", SubMenuName = "Все" }}
                        },
                        
                        //MainMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_car"], MenuText="AddCar", MenuName = "Автомобили"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="AddCar", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_change"], SubMenuText="СhangeCar", SubMenuName = "Изменить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="AllCars", SubMenuName = "Все" }}
                        },

                        //MainMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_employee"], MenuText="AddEmployee", MenuName = "Сотрудники"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="AddEmployee", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_change"], SubMenuText="СhangeEmployee", SubMenuName = "Изменить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="AllEmployees", SubMenuName = "Все" }}
                        },
                    };
                }
                else
                {
                    return new List<MenuItemsData>
                    {
                        //MainMenu without SubMenu Button 
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_policy"], MenuText="Policy", MenuName = "Полис"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="New", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="All", SubMenuName = "Добавить" }}},

                        //MainMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_policyholder"], MenuText="Policyholder", MenuName = "Страхователь"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="New", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="All", SubMenuName = "Весь список" }}
                        },

                        //MainMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_people"], MenuText="PersonAllowedToDrive", MenuName = "Люди", SubMenuList=null},

                         //MainMenu without SubMenu Button
                        new MenuItemsData(){ PathData= (PathGeometry)dict["icon_car"], MenuText="Car", MenuName = "Автомобиль"
                        //SubMenu Button
                        , SubMenuList=new List<SubMenuItemsData>{
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_add"], SubMenuText="New", SubMenuName = "Добавить" },
                        new SubMenuItemsData(){ PathData=(PathGeometry)dict["icon_all"], SubMenuText="All", SubMenuName = "Все страхователи" }}
                        },

                        
                    };
                }
            }
        }
    }

    public class MenuItemsData
    {
        //Icon Data
        public PathGeometry PathData { get; set; }
        public string MenuText { get; set; }
        public string MenuName { get; set; }
        public List<SubMenuItemsData> SubMenuList { get; set; }

        //To Add click event to our Buttons we will use ICommand here to switch pages when specific button is clicked
        public MenuItemsData()
        {
            Command = new CommandViewModel(Execute);
        }

        public ICommand Command { get; }

        private void Execute()
        {
            //our logic comes here
            string MT = MenuText.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(MT))
                navigateToPage(MT);
        }

        private void navigateToPage(string Menu)
        {
            //We will search for our Main Window in open windows and then will access the frame inside it to set the navigation to desired page..
            //lets see how... ;)
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
    public class SubMenuItemsData
    {
        public PathGeometry PathData { get; set; }
        public string SubMenuText { get; set; }
        public string SubMenuName { get; set; }

        //To Add click event to our Buttons we will use ICommand here to switch pages when specific button is clicked
        public SubMenuItemsData()
        {
            SubMenuCommand = new CommandViewModel(Execute);
        }

        public ICommand SubMenuCommand { get; }

        private void Execute()
        {
            //our logic comes here
            string SMT = SubMenuText.Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(SMT))
                navigateToPage(SMT);
        }

        private void navigateToPage(string Menu)
        {
            //We will search for our Main Window in open windows and then will access the frame inside it to set the navigation to desired page..
            //lets see how... ;)
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).MainWindowFrame.Navigate(new Uri(string.Format("{0}{1}{2}", "Pages/", Menu, ".xaml"), UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
}

using DCubeHotelDomain.Models;
using System.Collections.Generic;

namespace DCubeHotelSystem.Models
{
    public static class GetUserMenu
    {
        private static List<UserMenu> listMeun = new List<UserMenu>();
        public static List<UserMenu> getlistmeun()
        {
            if (listMeun.Count == 0)
            {
                listMeun.Add(new UserMenu(1, "Front Office", null, null, 0, "Front Office", ""));
                listMeun.Add(new UserMenu(2, "POS", null, null, 0, "POS", ""));
                listMeun.Add(new UserMenu(3, "Account", null, null, 0, "Account", ""));
                listMeun.Add(new UserMenu(4, "Inventory", null, null, 0, "Inventory", ""));
                listMeun.Add(new UserMenu(5, "Management", null, null, 0, "Management", ""));

                //Front Office

                //POS
                listMeun.Add(new UserMenu(200, "Order", "Index", "Order", 2, "Order", ""));
                listMeun.Add(new UserMenu(201, "Receipt", "Index", "Receipt", 2, "Receipt", ""));
                listMeun.Add(new UserMenu(202, "KOT/BOT", "Index", "KOTBOT", 2, "KOT BOT ", ""));
                listMeun.Add(new UserMenu(203, "KOT/BOT Merge", "Index", "KOTBOTMerge", 2, "KOT BOT Manage", ""));
                listMeun.Add(new UserMenu(204, "KOT/BOT Spilt", "Index", "KOTBOTSpilt", 2, "KOT BOT Spilt", ""));
                listMeun.Add(new UserMenu(205, "Customer", "Index", "Customer", 2, "Customer", ""));
                listMeun.Add(new UserMenu(206, "Report", null, null, 2, "Report", ""));
                listMeun.Add(new UserMenu(207, "Day Report", "Index", "DayReport", 206, "Day Report", ""));
                listMeun.Add(new UserMenu(208, "Item Sales Report", "Index", "ItemSalesReport", 206, "Item Sales Report", ""));
                listMeun.Add(new UserMenu(209, "Menu", null, null, 2, "Menu", ""));
                listMeun.Add(new UserMenu(210, "Comsumption", "Index", "MenuItemComsumption", 209, "Menu Item Comsumption", ""));
                listMeun.Add(new UserMenu(211, "Menu Item", "Index", "MenuItem", 209, "Menu Item", ""));
                listMeun.Add(new UserMenu(212, "Category", "Index", "MenuCategory", 209, "Menu Category", ""));
                listMeun.Add(new UserMenu(213, "Menu Type", "Index", "MenuType", 209, "Menu Type", ""));
                listMeun.Add(new UserMenu(214, "Table/Floor Design", "Index", "TableFloorDesign", 209, "Table/Floor Design", ""));
                listMeun.Add(new UserMenu(215, "Table/Floor Allocation", "Index", "TableFloorAllocation", 2, "Table/Floor Allocation", ""));
            }
            return listMeun;
        }
    }
}
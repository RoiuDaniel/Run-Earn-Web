using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web.Models
{
    public class ShopItemService
    {
        FirebaseClient firebaseClient;

        public ShopItemService()
        {
            firebaseClient = new FirebaseClient("https://runformoneydb-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public async void AddShopItem(ShopItem shopItem)
        {
            await firebaseClient.Child("ShopItem").PostAsync(shopItem);
        }

        public async Task<FirebaseObject<ShopItem>> GetShopItemByName(string name)
        {
            return (await firebaseClient.Child("ShopItem").OnceAsync<ShopItem>())
                .Where(a => a.Object.Name == name)
                .FirstOrDefault();
        }

        public async Task<FirebaseObject<ShopItem>> GetShopItemById(string id)
        {
            return (await firebaseClient.Child("ShopItem").OnceAsync<ShopItem>())
                .Where(a => a.Object.Id == id)
                .FirstOrDefault();
        }
        public async Task<List<FirebaseObject<ShopItem>>> GetAllShopItems()
        {
            return (await firebaseClient.Child("ShopItem").OnceAsync<ShopItem>()).ToList();
        }

        public async void UpdateShopItem(ShopItem shopItem)
        {
            FirebaseObject<ShopItem> item = await GetShopItemById(shopItem.Id);

            await firebaseClient
              .Child("ShopItem")
              .Child(item.Key)
              .PutAsync(shopItem);
        }

        public async void DeleteShopItem(string id)
        {
            FirebaseObject<ShopItem> item = await GetShopItemById(id);

            await firebaseClient.Child("ShopItem").Child(item.Key).DeleteAsync();
        }


        public async void DeleteShopItemByName(string name)
        {
            FirebaseObject<ShopItem> item = await GetShopItemByName(name);

            await firebaseClient.Child("ShopItem").Child(item.Key).DeleteAsync();
        }
    }
}
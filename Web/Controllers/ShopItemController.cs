using Firebase.Database;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ShopItemController : Controller
    {
        ShopItemService shopItemService = new ShopItemService();

        public async Task<ActionResult> Index()
        {
            List<ShopItem> shopItems = new List<ShopItem>();
            List<FirebaseObject<ShopItem>> items = await shopItemService.GetAllShopItems();
            foreach (FirebaseObject<ShopItem> firebaseObject in items)
            {
                shopItems.Add(firebaseObject.Object);
            }
            return View(shopItems);
        }
        // GET: ShopItem
        public ActionResult AddShopItem()
        {
            return View();
        }


        public async Task<ActionResult> Edit(string name)
        {
            FirebaseObject<ShopItem> shopItem = await shopItemService.GetShopItemByName(name);
            return View(shopItem.Object);
        }

        
        [HttpPost]
        public async Task<ActionResult> Edit(ShopItem shopItem, HttpPostedFileBase ImageFile)
        {
            if(ImageFile == null)
            {
                List<FirebaseObject<ShopItem>> items = await shopItemService.GetAllShopItems();
                foreach (FirebaseObject<ShopItem> firebaseObject in items)
                {
                    if(firebaseObject.Object.Id == shopItem.Id)
                    {
                        shopItem.ImgSource = firebaseObject.Object.ImgSource;
                    }
                }

                shopItemService.UpdateShopItem(shopItem);
            }
            else
            {
                FirebaseStorage firebaseStorage = new FirebaseStorage("runformoneydb.appspot.com");
                Guid imageName = Guid.NewGuid();
                var stroageImage = await firebaseStorage.Child("Images").Child(imageName.ToString()).PutAsync(ImageFile.InputStream);

                string c = await firebaseStorage.Child("Images").Child(imageName.ToString()).GetDownloadUrlAsync();
                shopItem.ImgSource = c;
                shopItemService.UpdateShopItem(shopItem);
            }
            await Task.Delay(500);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Delete(string name)
        {
            FirebaseObject<ShopItem> shopItem = await shopItemService.GetShopItemByName(name);
            return View(shopItem.Object);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ShopItem shopItem)
        {
            shopItemService.DeleteShopItemByName(shopItem.Name);
            await Task.Delay(500);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> ItemAdded(ShopItem shopItem, HttpPostedFileBase ImageFile)
        {
            string fileName = Path.GetFileName(ImageFile.FileName);
            FirebaseStorage firebaseStorage = new FirebaseStorage("runformoneydb.appspot.com");
            Guid imageName = Guid.NewGuid();
            Guid id = Guid.NewGuid();
            var stroageImage = await firebaseStorage.Child("Images").Child(imageName.ToString()).PutAsync(ImageFile.InputStream);

            string c = await firebaseStorage.Child("Images").Child(imageName.ToString()).GetDownloadUrlAsync();
            shopItem.Id = id.ToString();
            shopItem.ImgSource = c;
            shopItemService.AddShopItem(shopItem);
            await Task.Delay(500);
            return RedirectToAction("Index");
        }
    }
}
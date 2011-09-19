using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Client;


namespace MerchantTribe.Commerce.ApiSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is an API Sample Program for BV Commerce");
            Console.WriteLine();

            string url = string.Empty;
            string key = string.Empty;


            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    url = args[0];
                    key = args[1];
                }
            }

            if (url == string.Empty) url = "http://www.samplelocalhost6.com";
            if (key == string.Empty) key = "2-24f128dd-a492-410e-b8cb-b51adb06763c";


            Api proxy = new Api(url, key);


            //string toDelete = "71a802bb-2318-4182-8f95-614f8ad1333c";
            //ApiResponse<bool> response = proxy.CategoriesDelete(toDelete);
            //if (response.Content == true)
            //{
            //    Console.WriteLine("Deleted category with bvin of " + toDelete);
            //}

            //CategoryDTO c = new CategoryDTO();
            //c.Name = "A New Category From The Api";

            //ApiResponse<CategoryDTO> created = proxy.CategoriesCreate(c);

            //if (created == null) Console.WriteLine("Created response was null?!");

            //if (created != null)
            //{
            //    if (created.Errors.Count > 0)
            //    {
            //        foreach (ApiError err in created.Errors)
            //        {
            //            Console.WriteLine("Error: " + err.Description + "[" + err.Code + "]");
            //        }
            //    }
            //    if (created.Content != null)
            //    {
            //        string newBvin = created.Content.Bvin;
            //        Console.WriteLine("Created new Bvin of " + newBvin);
            //        created.Content.Name = created.Content.Name + " Updated!";
            //        ApiResponse<CategoryDTO> updated = proxy.CategoriesUpdate(created.Content);
            //        if (updated.Errors.Count > 0)
            //        {
            //            foreach (ApiError err in updated.Errors)
            //            {
            //                Console.WriteLine("Error: " + err.Description + "[" + err.Code + "]");
            //            }
            //        }
            //        if (updated.Content != null)
            //        {
            //            Console.WriteLine("Updated category to name of " + updated.Content.Name);
            //        }

            //    }

            //}


            //// SLUGIFY
            //Console.WriteLine("Enter a string to slugify:");
            //string input = Console.ReadLine();
            //ApiResponse<string> result = proxy.UtilitiesSlugify(input);
            //Console.WriteLine("Slugified=" + result.Content ?? "Result was null");

            ApiResponse<List<CategorySnapshotDTO>> snaps = proxy.CategoriesFindAll();
            if (snaps.Content != null)
            {
                Console.WriteLine("Found " + snaps.Content.Count + " categories");
                Console.WriteLine("-- First 5 --");
                for (int i = 0; i < 5; i++)
                {
                    if (i < snaps.Content.Count)
                    {
                        Console.WriteLine(i + ") " + snaps.Content[i].Name + " [" + snaps.Content[i].Bvin + "]");
                        ApiResponse<CategoryDTO> cat = proxy.CategoriesFind(snaps.Content[i].Bvin);
                        if (cat.Errors.Count > 0)
                        {
                            foreach (ApiError err in cat.Errors)
                            {
                                Console.WriteLine("ERROR: " + err.Code + " " + err.Description);
                            }
                        }
                        else
                        {
                            Console.WriteLine("By Bvin: " + cat.Content.Name + " | " + cat.Content.Description);
                        }

                        ApiResponse<CategoryDTO> catSlug = proxy.CategoriesFindBySlug(snaps.Content[i].RewriteUrl);
                        if (catSlug.Errors.Count > 0)
                        {
                            foreach (ApiError err in catSlug.Errors)
                            {
                                Console.WriteLine("ERROR: " + err.Code + " " + err.Description);
                            }
                        }
                        else
                        {
                            Console.WriteLine("By Slug: " + cat.Content.Name + " | " + cat.Content.Description);
                        }
                    }
                }
            }

            Console.WriteLine("Done - Press a key to continue");
            Console.ReadKey();
        }
    }
}

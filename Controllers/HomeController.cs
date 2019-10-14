using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        public Random rand = new Random();
        public IActionResult Index()
        {
            
            if(HttpContext.Session.GetString("Name") == null)
            {
                Character friend = new Character();
                HttpContext.Session.SetString("Name", friend.Name);
                HttpContext.Session.SetInt32("Fullness", friend.Fullness);
                HttpContext.Session.SetInt32("Happiness", friend.Happiness);
                HttpContext.Session.SetInt32("Energy", friend.Energy);
                HttpContext.Session.SetInt32("Meals", friend.Meals);
                HttpContext.Session.SetString("alive", "alive");
                HttpContext.Session.SetString("message", "");
                HttpContext.Session.SetString("Status", "Friend is okay");
                
            }
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            int? energy = HttpContext.Session.GetInt32("Energy");
            int? meals = HttpContext.Session.GetInt32("Meals");
            string message= HttpContext.Session.GetString("message");
            string alive = HttpContext.Session.GetString("alive");
           

            ViewBag.Fullness = (int)fullness;
            ViewBag.Happiness = (int)happiness;
            ViewBag.Energy = (int)energy;
            ViewBag.Meals = (int)meals;
            ViewBag.message = message;
            ViewBag.alive = alive;
            
            
            if((int)fullness >= 100 && (int)happiness >= 100 && (int)energy >= 100)
            {
                HttpContext.Session.SetString("alive", "Win");
                ViewBag.alive = "win";
                HttpContext.Session.SetString("message", "You won! Restart?");
            }
            else if((int)happiness <= 0 || (int)fullness <= 0 || (int)energy <= 0)
            {
                HttpContext.Session.SetString("alive", "Dead");
                ViewBag.alive = "dead";
                HttpContext.Session.SetString("message", "Your friend has died! Restart?");
            }
            return View();
        }

        public IActionResult Feed()
        {
            int chance = rand.Next(0,100);
            int? meals = HttpContext.Session.GetInt32("Meals");
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            if(chance <= 25)
            {
                meals --;
                HttpContext.Session.SetInt32("Meals", (int)meals);
                HttpContext.Session.SetString("message", "Your friend didn't like what you fed it. You lost a meal but they are still hungry.");
            }
            if(meals > 0)
            {
                fullness += rand.Next(5,10);
                meals --;
                HttpContext.Session.SetInt32("Fullness", (int)fullness);
                HttpContext.Session.SetInt32("Meals", (int)meals);
                HttpContext.Session.SetString("message", $"You fed your friend! They are at {fullness} fullness");
            }
            if(meals == 0)
            {
                meals = 0;
                HttpContext.Session.SetString("message", "You have run out of meals, work to get more!");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Play()
        {
            int chance = rand.Next(0,100);
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            int? energy = HttpContext.Session.GetInt32("Energy");
            if(chance <= 25)
            {
                energy -= 5;
                HttpContext.Session.SetInt32("Energy", (int)energy);
                HttpContext.Session.SetString("message", "Your friend didn't enjoy your company. Their energy decreased but they are not happier.");
            }
            else
            {
                energy -= 5;
                happiness += rand.Next(5,10);
                HttpContext.Session.SetInt32("Happiness", (int)happiness);
                HttpContext.Session.SetInt32("Energy", (int)energy);
                HttpContext.Session.SetString("message", $"You played with your friend and their happiness went up to {happiness}");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Work()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            int? meals = HttpContext.Session.GetInt32("Meals");
            energy -= 5;
            meals += rand.Next(1,3);
            HttpContext.Session.SetInt32("Energy", (int)energy);
            HttpContext.Session.SetInt32("Meals", (int)meals);
            HttpContext.Session.SetString("message", $"Your friend worked and lost 5 energy and now has {meals} meals.");
            return RedirectToAction("Index");
        }

        public IActionResult Sleep()
        {
            int? energy = HttpContext.Session.GetInt32("Energy");
            int? fullness = HttpContext.Session.GetInt32("Fullness");
            int? happiness = HttpContext.Session.GetInt32("Happiness");
            energy += 15;
            fullness -= 5;
            happiness -= 5;
            HttpContext.Session.SetInt32("Energy", (int)energy);
            HttpContext.Session.SetInt32("Fullness", (int)fullness);
            HttpContext.Session.SetInt32("Happiness", (int)happiness);
            HttpContext.Session.SetString("message", $"Your friend took a nap and is now at {energy} energy. Time to play?");
            return RedirectToAction("Index");
        }

        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

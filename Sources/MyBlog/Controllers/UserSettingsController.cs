using MyBlog.Engine;
using MyBlog.Models;
using MyLib.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    
    [XRobotsTagNoIndex]
    public class UserSettingsController : Controller
    {
        [HttpPost]
        public ActionResult CookiesConcentClosed()
        {
            // Get current settings
            var settings = UserSettingsService.Get();
            // Set new value
            settings.CookiesConcentClosed = true;
            // Save settings
            UserSettingsService.Set(settings);
            return Json("true");
        }

        #region Manage Accessibility settings

        [HttpGet]
        public ViewResult Accessibility()
        {
            return View(GetAccessibilityModel());
        }
        
        [HttpPost]
        public PartialViewResult _Accessibility()
        {
            return PartialView("_Accessibility", GetAccessibilityModel());
        }


        private static AccessibilitySettings GetAccessibilityModel()
        {
            // Get current settings
            var settings = UserSettingsService.Get();

            AccessibilitySettings model = new AccessibilitySettings
            {
                UseDyslexicFont = settings.UseDyslexicFont
            };
            return model;
        }

        [HttpPost]
        public JsonResult SetDefaultFont()
        {
            // Get current settings
            var settings = UserSettingsService.Get();
            // Set new value
            settings.UseDyslexicFont = false;
            // Save settings
            UserSettingsService.Set(settings);
            return Json(settings.LayoutContentUrl);
        }

        [HttpPost]
        public JsonResult SetDyslexicFont()
        {
            // Get current settings
            var settings = UserSettingsService.Get();
            // Set new value
            settings.UseDyslexicFont = true;
            // Save settings
            UserSettingsService.Set(settings);
            return Json(settings.LayoutContentUrl);
        }

        #endregion
    }
}
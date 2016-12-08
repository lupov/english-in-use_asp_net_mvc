/**************************************************************************
 * Copiright(C) 2016 Sergey Lupov
 *
 * English-in-Use (v.1.X.X) is an application for studying english. 
 * Prepared english phrases are spoken in a random order.
 * You have to recall the translation of the phrase
 * and after a short pause they are spoken by the program automatically.
 *
 * Web Site: http://english-in-use.com	
 * E-mail: sergey.lupov {at} english-in-use.com
 *
 * This file is part of English-in-Use.
 * 
 * English-in-Use is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Foobar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using English_in_Use.Models.DataReadUtils;
using English_in_Use.Models.Phrases;
using System.Threading;

namespace English_in_Use.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        [HttpGet]
        public ActionResult Index(string startBtn, string stopBtn, string dir, string Period, string Words)
        {
            if (Session["ViewParams"] == null) Session["ViewParams"] = new ViewParameters();
            ViewParameters viewParams = Session["ViewParams"] as ViewParameters;
            if (dir == "En") viewParams.En = "checked";
            else if (dir == "RuEn") viewParams.RuEn = "checked";
            else if (dir == "EnRu") viewParams.EnRu = "checked";
            if (Period != null) viewParams.Slow = "checked";
            else if(startBtn != null) viewParams.Slow = "";
            if (Words != null) viewParams.Words = Words;

//            Verbs = new VerbsCollection(dataReader);
            ViewBag.Verbs = Verbs;
            if (startBtn != null) ViewBag.showAction = true;
            else ViewBag.showAction = false;

            return View(viewParams);
        }
        
        [HttpGet]
        public ActionResult PartialIndex(string startBtn, string dir, string Period)
        {
            if (Period == "Slow") ViewBag.Delay = 8000;
            else ViewBag.Delay = 3000;
            ViewParameters viewParams = Session["ViewParams"] as ViewParameters;
            List<int> indexesSel = new List<int>();
            for(int i=0; i<viewParams.Words.Length; i++)
            {
                if(viewParams.Words[i]=='1') indexesSel.Add(i);
            }
            Phrase phrase = Verbs.GetRandomVerb(indexesSel).GetRandomPhrase();
            if (dir == "EnRu")       return PartialView("EnRuAudio", phrase);
            else if (dir == "RuEn") return PartialView("RuEnAudio", phrase);
            else return PartialView("EnAudio", phrase);
        }

        public static VerbsCollection Verbs = new VerbsCollection(new xmlDataReader("Lesson1.xml"));
    }

    public class ViewParameters
    {
        public ViewParameters() 
        { 
            EnRu = "checked";
            Slow = "";
            Words = "";
            for(int i=0; i<MainController.Verbs.verbs.Count; i++) Words += "1";

        }
        public string EnRu
        {
            set
            {
                _enru = value;
                _ruen = "";
                _en = "";

            }
            get { return _enru; }
        }
        public string RuEn
        {
            set
            {
                _ruen = value;
                _enru = "";
                _en = "";

            }
            get { return _ruen; }
        }
        public string En
        {
            set
            {
                _en = value;
                _enru = "";
                _ruen = "";

            }
            get { return _en; }
        }
        public string Words { set; get; }

        public string Slow { set; get; }

        private string _enru;
        private string _ruen;
        private string _en;
    };
}

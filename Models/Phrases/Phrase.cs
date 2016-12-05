﻿/**************************************************************************
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

namespace English_in_Use.Models.Phrases
{
    public class Phrase
    {
        public Phrase(String engPhrase, String rusPhrase, String FileName)
        {
            this.engPhrase = engPhrase;
            this.rusPhrase = rusPhrase;
            this.FileName = FileName;
        }
        public String engPhrase;
        public String rusPhrase;
        public String FileName;
    }
}
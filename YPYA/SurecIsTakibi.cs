﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YPYA
{
    public class SurecIstakibi
    {
        public string surecNote { get; set; }

        public string analizKisi { get; set; }
        public DateTime analizBaslangicTarihi { get; set; }
        public DateTime analizBitisTarihi { get; set; }
        public DateTime analizTamamlanmaTarihi { get; set; }
        public int analizTamamlanmaOrani { get; set; }

        public string tableKisi { get; set; }
        public DateTime tableBaslangicTarihi { get; set; }
        public DateTime tableBitisTarihi { get; set; }
        public DateTime tableTamamlanmaTarihi { get; set; }
        public int tableTamamlanmaOrani { get; set; }

        public string procedureKisi { get; set; }
        public DateTime procedureBaslangicTarihi { get; set; }
        public DateTime procedureBitisTarihi { get; set; }
        public DateTime procedureTamamlanmaTarihi { get; set; }
        public int procedureTamamlanmaOrani { get; set; }

        public string dllListKisi { get; set; }
        public DateTime dllListBaslangicTarihi { get; set; }
        public DateTime dllListBitisTarihi { get; set; }
        public DateTime dllListTamamlanmaTarihi { get; set; }
        public int dllListTamamlanmaOrani { get; set; }

        public string dllIslemKisi { get; set; }
        public DateTime dllIslemBaslangicTarihi { get; set; }
        public DateTime dllIslemBitisTarihi { get; set; }
        public DateTime dllIslemTamamlanmaTarihi { get; set; }
        public int dllIslemTamamlanmaOrani { get; set; }

        public string arayuzKisi { get; set; }
        public DateTime arayuzBaslangicTarihi { get; set; }
        public DateTime arayuzBitisTarihi { get; set; }
        public DateTime arayuzTamamlanmaTarihi { get; set; }
        public int arayuzTamamlanmaOrani { get; set; }

        public string testKisi { get; set; }
        public DateTime testBaslangicTarihi { get; set; }
        public DateTime testBitisTarihi { get; set; }
        public DateTime testTamamlanmaTarihi { get; set; }
        public int testTamamlanmaOrani { get; set; }

    }
}
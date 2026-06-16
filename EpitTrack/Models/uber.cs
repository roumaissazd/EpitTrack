using EpitTrack.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using OfficeOpenXml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EpitTrack.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EpitTrack.Models
{
    [Table("db_ubercourses", Schema = "public")]
    public class uber
    {
        [Key]
        [Required]
        public string id_transact { set; get; }
        public string id_chauffeur { set; get; }
        public string prenom_chauff { set; get; }
        public string nom_chauff { set; get; }
        public string id_course { set; get; }
        public string description { set; get; }
        public string nom_organisation { set; get; }
        public string alias_organisation { set; get; }
        public string date_creation { set; get; }
        public double mt_verse { set; get; }
        public double mt_revenus { set; get; }
        public double mt_especes { set; get; }
        public double mt_tarif_course { set; get; }
        public double mt_taxes { set; get; }
        public double mt_tarif_dynam { set; get; }
        public double mt_fr_service { set; get; }
        public double mt_fr_resa { set; get; }
        public double mt_pour_boire { set; get; }
        public double mt_prise_en_charge { set; get; }

        public uber()
        { }


        public bool ImportUber(IFormFile file, AppDbContext _context)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            MemoryStream stream = new MemoryStream();

            file.CopyTo(stream);
            ExcelPackage package = new ExcelPackage(stream);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); //

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var uberCourse = new uber()
                {
                    id_transact = worksheet.Cells[row, 1].Text,
                    id_chauffeur = worksheet.Cells[row, 2].Text,
                    prenom_chauff = worksheet.Cells[row, 3].Text,
                    nom_chauff = worksheet.Cells[row, 4].Text,
                    id_course = worksheet.Cells[row, 5].Text,
                    description = worksheet.Cells[row, 6].Text,
                    nom_organisation = worksheet.Cells[row, 7].Text,
                    alias_organisation = worksheet.Cells[row, 8].Text,
                    date_creation = worksheet.Cells[row, 9].Text.Substring(0, 16),
                    mt_verse = double.Parse(worksheet.Cells[row, 10].Text),
                    mt_revenus = double.Parse(worksheet.Cells[row, 11].Text),
                    mt_especes = double.Parse(worksheet.Cells[row, 12].Text),
                    mt_tarif_course = double.Parse(worksheet.Cells[row, 13].Text),
                    mt_taxes = double.Parse(worksheet.Cells[row, 14].Text),
                    mt_tarif_dynam = double.Parse(worksheet.Cells[row, 15].Text),
                    mt_fr_service = double.Parse(worksheet.Cells[row, 16].Text),
                    mt_fr_resa = double.Parse(worksheet.Cells[row, 17].Text),
                    mt_pour_boire = double.Parse(worksheet.Cells[row, 18].Text),
                    mt_prise_en_charge = double.Parse(worksheet.Cells[row, 10].Text)
                };
                _context.ubers.Add(uberCourse);
            };
                        

            _context.SaveChanges();


                return true;


            }
        }
    }




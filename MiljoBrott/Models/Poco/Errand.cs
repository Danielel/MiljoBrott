using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class Errand
	{
		public int ErrandID { get; set; }

		//RefNumber form: år-45-löpnummer
		public string RefNumber { get; set; }

		[Required(ErrorMessage = "Du måste skriva in en plats")]
		public string Place { get; set; }
		[Required(ErrorMessage = "Du måste skriva typen av brott")]
		public string TypeOfCrime { get; set; }


		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required(ErrorMessage = "Du måste skriva in ett datum")]
		//regex från https://regexlib.com/REDetails.aspx?regexp_id=190
		[RegularExpression(pattern: @"^(?:(?:(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(\/|-|\.)(?:0?2\1(?:29)))|(?:(?:(?:1[6-9]|[2-9]\d)?\d{2})(\/|-|\.)(?:(?:(?:0?[13578]|1[02])\2(?:31))|(?:(?:0?[1,3-9]|1[0-2])\2(29|30))|(?:(?:0?[1-9])|(?:1[0-2]))\2(?:0?[1-9]|1\d|2[0-8]))))$", ErrorMessage = "Datum skrivs som år-månad-dag")]
		public DateTime DateOfObservation { get; set; }
		public string Observation { get; set; }
		public string InvestigatorInfo { get; set; }
		public string InvestigatorAction { get; set; }

		[Required(ErrorMessage = "Du måste skrive in förnamn och efternamn")]
		public string InformerName { get; set; }
		[Required(ErrorMessage = "Du måste skriva in ditt telefonnummer")]
		[RegularExpression(pattern: @"^[0]{1}[0-9]{1,3}-[0-9]{5,9}$", ErrorMessage = "Formatet är riktnummer-telefonnummer")]
		public string InformerPhone { get; set; }
		public string StatusId { get; set; }
		public string DepartmentId { get; set; }
		public string EmployeeId { get; set; }

		public ICollection<Sample> Samples { get; set; }
		public ICollection<Picture> Pictures { get; set; }
	}
}

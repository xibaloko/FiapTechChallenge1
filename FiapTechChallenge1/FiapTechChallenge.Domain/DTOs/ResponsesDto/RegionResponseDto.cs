using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapTechChallenge.Domain.DTOs.ResponsesDto
{
    public class RegionResponseDto
    {
        public int RegionId { get; set; }
        public required string RegionName { get; set; }
    }
}

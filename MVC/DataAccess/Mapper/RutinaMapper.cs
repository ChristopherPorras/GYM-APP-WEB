using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Mapper
{
    public class RutinaMapper : MapperBase<RutinaDTO>
    {
        public override RutinaDTO BuildObject(IDataReader reader)
        {
            return new RutinaDTO
            {
                ID = Convert.ToInt32(reader["ID"]),
                CorreoElectronico = reader["CorreoElectronico"].ToString(),
                MedicionId = Convert.ToInt32(reader["MedicionId"]),
                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                EntrenadorCorreo = reader["EntrenadorCorreo"].ToString()
            };
        }

        public override RutinaDTO BuildObject(Dictionary<string, object> row)
        {
            return new RutinaDTO
            {
                ID = Convert.ToInt32(row["ID"]),
                CorreoElectronico = row["CorreoElectronico"].ToString(),
                MedicionId = Convert.ToInt32(row["MedicionId"]),
                FechaCreacion = Convert.ToDateTime(row["FechaCreacion"]),
                EntrenadorCorreo = row["EntrenadorCorreo"].ToString()
            };
        }

        public override List<RutinaDTO> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var list = new List<RutinaDTO>();

            foreach (var row in lstRows)
            {
                list.Add(BuildObject(row));
            }

            return list;
        }
    }
}

using Dapper;
using PSMDataManager.Library.Internal.DataAccess;
using PSMDataManager.Library.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSMDataManager.Library.DataAccess
{
    public class ServiceData
    {
        public List<ServiceModel> GetServices()
        {
            SqlDataAccess sql = new SqlDataAccess();

            List<ServiceModel> data = sql.LoadData<ServiceModel, dynamic>("dbo.spGetAllServices", new { }, "PSMData");
            return data;
        }

        public ServiceModel GetServiceByNomorNota(int nomorNota)
        {
            SqlDataAccess sql = new SqlDataAccess();
            ServiceModel service = sql.LoadData<ServiceModel, dynamic>("dbo.spGetService", new { NomorNota = nomorNota }, "PSMData").FirstOrDefault();

            return service;
        }

        public int InsertService(ServiceModel service)
        {
            SqlDataAccess sql = new SqlDataAccess();
            const string nomorNotaCol = "NomorNota";

            var p = new 
            {
                service.NamaPelanggan,
                service.NoHp,
                service.TipeHp,
                service.Imei,
                service.Kerusakan,
                service.KondisiHp,
                service.YangBelumDicek,
                service.Kelengkapan,
                service.Warna,
                service.KataSandiPola,
                service.TechnicianId,
                service.SalesId,
                service.StatusServisan,
                service.TanggalKonfirmasi,
                service.IsiKonfirmasi,
                service.Biaya,
                service.Discount,
                service.Dp,
                service.TambahanBiaya,
                service.HargaSparepart,
                service.TanggalPengambilan
            };

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(nomorNotaCol, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.AddDynamicParams(p);

            sql.SaveData("dbo.spInsertService", parameters, "PSMData");
            return parameters.Get<int>(nomorNotaCol);
        }

        public void UpdateService(ServiceModel service)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new
            {
                service.NomorNota,
                service.NamaPelanggan,
                service.NoHp,
                service.TipeHp,
                service.Imei,
                service.Kerusakan,
                service.KondisiHp,
                service.YangBelumDicek,
                service.Kelengkapan,
                service.Warna,
                service.KataSandiPola,
                service.TechnicianId,
                service.SalesId,
                service.StatusServisan,
                service.TanggalKonfirmasi,
                service.IsiKonfirmasi,
                service.Biaya,
                service.Discount,
                service.Dp,
                service.TambahanBiaya,
                service.TanggalPengambilan
            };

            sql.SaveData<dynamic>("dbo.spUpdateService", p, "PSMData");
        }

        public void DeleteService(int nomorNota)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var p = new { NomorNota = nomorNota };

            sql.SaveData<dynamic>("dbo.spDeleteService", p, "PSMData");
        }
    }
}

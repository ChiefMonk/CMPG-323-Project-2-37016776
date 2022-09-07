using System;
using System.Collections.Generic;
using System.Linq;
using Project2.WebAPI.DAL.Dtos;
using Project2.WebAPI.DAL.Entities;

namespace Project2.WebAPI.DAL.Converters
{
    internal static class DeviceConverters
    {
        #region ToDto

        internal static DtoDevice ToDtoDevice(this EntityDevice entity)
        {
            if (entity == null)
                return null;

            return new DtoDevice
            {
                Id = entity.DeviceId,
                DeviceName = entity.DeviceName,
                CategoryId = entity.CategoryId,
                ZoneId = entity.ZoneId,
                Status = entity.Status,
                IsActive = entity.IsActvie,
                DateCreated = entity.DateCreated,
            };
        }

        internal static IList<DtoDevice> ToDtoDeviceCollection(this IEnumerable<EntityDevice> entityList)
        {
            var dtoList = new List<DtoDevice>();

            if (entityList == null || !entityList.Any())
                return dtoList;

            foreach (var entity in entityList)
            {
                var dto = entity.ToDtoDevice();
                if (dto != null)
                    dtoList.Add(dto);
            }

            return dtoList;
        }

        #endregion

        #region ToEntity

        internal static EntityDevice ToEntityDevice(this DtoDevice dto)
        {
            if (dto == null)
                return null;

            return new EntityDevice
            {
                DeviceId = dto.Id,
                DeviceName = dto.DeviceName,
                CategoryId = dto.CategoryId,
                ZoneId = dto.ZoneId,
                Status = dto.Status,
                IsActvie = dto.IsActive,
                DateCreated = dto.DateCreated,
            };
        }

        internal static EntityDevice ToEntityDevice(this DtoDevice dto, EntityDevice currentEntity)
        {
            if (dto == null)
                return null;

            currentEntity.DeviceName = dto.DeviceName;

            if (dto.CategoryId != Guid.NewGuid())
                currentEntity.CategoryId = dto.CategoryId;

            if (dto.ZoneId != Guid.NewGuid())
                currentEntity.ZoneId = dto.ZoneId;

            currentEntity.Status = dto.Status;
            currentEntity.IsActvie = dto.IsActive;

            return currentEntity;
        }

        #endregion
    }
}

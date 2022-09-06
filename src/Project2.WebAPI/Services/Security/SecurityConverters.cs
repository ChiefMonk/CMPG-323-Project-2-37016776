using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Project2.Data.Entities;
using Project2.WebAPI.Dtos;

namespace Project2.WebAPI.Services.Security
{
	internal static class SecurityConverters
	{
		#region ToDto

		internal static DtoUserAuthenticationResponse ToDtoSystemUser(this EntitySystemUser entity, string roleName, JwtSecurityToken jwtSecurityToken)
		{
			if (entity == null)
				return null;

			return new DtoUserAuthenticationResponse
			{
				User = entity.ToDtoSystemUser(roleName),
				JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				JwtExpiry = jwtSecurityToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss")
			};
		}

		internal static DtoSystemUser ToDtoSystemUser(this EntitySystemUser entity, string roleName)
		{
			if (entity == null)
				return null;

			return new DtoSystemUser
			{
				Id = entity.Id,
				UserName = entity.UserName,
				EmailAddress = entity.Email,
				PhoneNumber = entity.PhoneNumber,
				UtcOffset = 2.0,
				RoleName = roleName
			};
		}

		internal static IList<DtoSystemUser> ToDtoSystemUserCollection(this IEnumerable<EntitySystemUser> entityList)
		{
			var dtoList = new List<DtoSystemUser>();

			if (entityList == null || !entityList.Any())
				return dtoList;

			foreach (var entity in entityList)
			{
				var dto = entity.ToDtoSystemUser("");
				if (dto != null)
					dtoList.Add(dto);
			}

			return dtoList;
		}

		#endregion

		#region ToEntity

		internal static EntitySystemUser ToEntitySystemUser(this DtoSystemUser dto)
		{
			if (dto == null)
				return null;

			return new EntitySystemUser
			{
				UserName = dto.UserName,
				Email = dto.EmailAddress,
				PhoneNumber = dto.PhoneNumber,

				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				TwoFactorEnabled = false,
				LockoutEnabled = false,
			};
		}

		internal static EntitySystemUser ToEntitySystemUser(this DtoUserRegistrationRequest request)
		{
			if (request == null)
				return null;

			return new EntitySystemUser
			{
				UserName = request.UserName,
				Email = request.EmailAddress,
				PhoneNumber = request.PhoneNumber,
				SecurityStamp = Guid.NewGuid().ToString(),
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
				TwoFactorEnabled = false,
				LockoutEnabled = false,
			};
		}

		#endregion
	}
}

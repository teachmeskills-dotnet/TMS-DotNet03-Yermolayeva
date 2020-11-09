using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    public class ProfileManager : IProfileManager
    {
        private readonly IRepository<Profile> _repositoryProfile;

        public ProfileManager(IRepository<Profile> repositoryProfile)
        {
            _repositoryProfile = repositoryProfile ?? throw new ArgumentNullException(nameof(repositoryProfile));
        }

        public async System.Threading.Tasks.Task CreateAsync(ProfileDto profileDto)
        {
            var profile = new Profile
            {
                UserId = profileDto.UserId,
                Created = DateTime.Now,
                IsVendor = profileDto.IsVendor,
                Info = null,
                Name = profileDto.Name
            };

            await _repositoryProfile.CreateAsync(profile);
            await _repositoryProfile.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id, string userId)
        {
            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.Id == id && profile.UserId == userId);
            if (profile is null)
            {
                return;
            }

            _repositoryProfile.Delete(profile);
            await _repositoryProfile.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProfileDto>> GetAllVendorProfiles()
        {
            var profileDtos = new List<ProfileDto>();
            var profiles = await _repositoryProfile
                .GetAll()
                .AsNoTracking()
                .Where(profile => profile.IsVendor)
                .ToListAsync();

            if (!profiles.Any())
            {
                return profileDtos;
            }

            foreach (var profile in profiles)
            {
                profileDtos.Add(new ProfileDto
                {
                    Id = profile.Id,
                    UserId = profile.UserId,
                    Created = profile.Created,
                    IsVendor = profile.IsVendor,
                    Info = profile.Info,
                    Name = profile.Name
                });
            }

            return profileDtos;
        }

        public async Task<ProfileDto> GetProfileAsync(int id, string userId)
        {
            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.Id == id && profile.UserId == userId);
            if (profile is null)
            {
                return null;
            }

            var profileDto = new ProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Created = profile.Created,
                IsVendor = profile.IsVendor,
                Info = profile.Info,
                Name = profile.Name
            };
            return profileDto;
        }

        public async System.Threading.Tasks.Task UpdateProfileAsync(ProfileDto profileDto)
        {
            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.Id == profileDto.Id && profile.UserId == profileDto.UserId);
            if (profile is null)
            {
                return;
            }

            static bool ValidateToUpdate(Profile profile, ProfileDto profileDto)
            {
                bool updated = false;

                if (profile.Name != profileDto.Name)
                {
                    profile.Name = profileDto.Name;
                    updated = true;
                }

                if (profile.Info != profileDto.Info)
                {
                    profile.Info = profileDto.Info;
                    updated = true;
                }

                if (profile.IsVendor != profileDto.IsVendor)
                {
                    profile.IsVendor = profileDto.IsVendor;
                    updated = true;
                }

                return updated;
            }

            var result = ValidateToUpdate(profile, profileDto);
            if (result)
            {
                await _repositoryProfile.SaveChangesAsync();
            }
        }
    }
}

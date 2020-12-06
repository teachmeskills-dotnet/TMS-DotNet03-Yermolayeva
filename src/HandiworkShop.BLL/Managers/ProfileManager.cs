using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    public class ProfileManager : IProfileManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Profile> _repositoryProfile;
        private readonly IRepository<UserTag> _repositoryUserTag;

        public ProfileManager(UserManager<ApplicationUser> userManager, 
            IRepository<Profile> repositoryProfile, 
            IRepository<UserTag> repositoryUserTag)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _repositoryProfile = repositoryProfile ?? throw new ArgumentNullException(nameof(repositoryProfile));
            _repositoryUserTag = repositoryUserTag ?? throw new ArgumentNullException(nameof(repositoryUserTag));
        }

        public async System.Threading.Tasks.Task CreateAsync(ProfileDto profileDto)
        {
            profileDto = profileDto ?? throw new ArgumentNullException(nameof(profileDto));

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

            if (profileDto.TagIds.Any())
            {
                foreach (var tagId in profileDto.TagIds)
                {
                    await _repositoryUserTag.CreateAsync(new UserTag()
                    {
                        UserId = profileDto.UserId,
                        TagId = tagId
                    });
                }
                await _repositoryUserTag.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id, string userId)
        {
            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.Id == id && profile.UserId == userId);
            if (profile is null)
            {
                throw new KeyNotFoundException();
            }

            _repositoryProfile.Delete(profile);
            await _repositoryProfile.SaveChangesAsync();
        }

        public async Task<ProfileDto> GetProfileAsync(string userId)
        {
            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.UserId == userId);
            if (profile is null)
            {
                throw new KeyNotFoundException();
            }

            var profileDto = new ProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Created = profile.Created,
                IsVendor = profile.IsVendor,
                Info = profile.Info,
                Name = profile.Name,
                Avatar = profile.Avatar
            };
            return profileDto;
        }

        public async Task<IEnumerable<ProfileDto>> GetProfilesByTagsAsync(IList<int> tagIds)
        {
            var profileDtos = new List<ProfileDto>();
            var profiles = new List<Profile>();

            if (tagIds != null && tagIds.Any())
            {
                var userIds = await _repositoryUserTag
                    .GetAll()
                    .AsNoTracking()
                    .Where(userTag => tagIds.Contains(userTag.TagId))
                    .Select(userTag => userTag.UserId)
                    .ToListAsync();

                userIds = userIds.GroupBy(id => id).Where(group => group.Count() == tagIds.Count()).Select(group => group.First()).ToList() ;

                profiles = await _repositoryProfile
                    .GetAll()
                    .AsNoTracking()
                    .Where(profile => userIds.Contains(profile.UserId) && profile.IsVendor)
                    .ToListAsync();           
            }
            else
            {
                profiles = await _repositoryProfile
                    .GetAll()
                    .AsNoTracking()
                    .Where(profile => profile.IsVendor)
                    .ToListAsync();
            }

            if (profiles.Any())
            {
                foreach (var profile in profiles)
                {
                    profileDtos.Add(new ProfileDto
                    {
                        Id = profile.Id,
                        UserId = profile.UserId,
                        Created = profile.Created,
                        IsVendor = profile.IsVendor,
                        Info = profile.Info,
                        Name = profile.Name,
                        Avatar = profile.Avatar
                    });
                }
            }
            
            return profileDtos;
        }

        public async System.Threading.Tasks.Task UpdateProfileAsync(ProfileDto profileDto, string userId)
        {
            profileDto = profileDto ?? throw new ArgumentNullException(nameof(profileDto));

            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.Id == profileDto.Id && profile.UserId == userId);

            if (profile is null)
            {
                throw new KeyNotFoundException();
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

                if (profile.Avatar != profileDto.Avatar && profileDto.Avatar != null)
                {
                    profile.Avatar = profileDto.Avatar;
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

        public async System.Threading.Tasks.Task SwitchProfileStatusAsync(string userId)
        {
            var profile = await _repositoryProfile.GetEntityAsync(profile => profile.UserId == userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (profile is null)
            {
                throw new KeyNotFoundException();
            }

            profile.IsVendor = !profile.IsVendor;

            if (await _userManager.IsInRoleAsync(user, "Vendor"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Vendor");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Vendor");
            }
            await _repositoryProfile.SaveChangesAsync();
        }
    }
}

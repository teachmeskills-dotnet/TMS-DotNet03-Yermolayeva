using HandiworkShop.BLL.Interfaces;
using HandiworkShop.BLL.Models;
using HandiworkShop.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandiworkShop.BLL.Managers
{
    public class TagManager : ITagManager
    {
        private readonly IRepository<Tag> _repositoryTag;
        private readonly IRepository<UserTag> _repositoryUserTag;

        public TagManager(IRepository<Tag> repositoryTag, IRepository<UserTag> repositoryUserTag)
        {
            _repositoryTag = repositoryTag ?? throw new ArgumentNullException(nameof(repositoryTag));
            _repositoryUserTag = repositoryUserTag ?? throw new ArgumentNullException(nameof(repositoryUserTag));
        }

        public async System.Threading.Tasks.Task AddUserTagAsync(int id, string userId)
        {
            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == id);

            if (tag is null)
            {
                return;
            }

            var userTag = new UserTag
            {
                TagId = id,
                UserId = userId
            };

            await _repositoryUserTag.CreateAsync(userTag);
            await _repositoryUserTag.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task CreateAsync(TagDto tagDto)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == id);
            if (tag is null)
            {
                return;
            }

            _repositoryTag.Delete(tag);
            await _repositoryTag.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteUserTagAsync(int id, string userId)
        {
            var userTag = new UserTag
            {
                TagId = id,
                UserId = userId
            };

            _repositoryUserTag.Delete(userTag);
            await _repositoryUserTag.SaveChangesAsync();
        }

        public async Task<TagDto> GetTagAsync(int id)
        {
            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == id);
            if (tag is null)
            {
                return null;
            }

            var tagDto = new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
            return tagDto;
        }

        public async Task<IEnumerable<TagDto>> GetTagsAsync(string userId)
        {
            var tagDtos = new List<TagDto>();

            var tagIds = await _repositoryUserTag
                .GetAll()
                .AsNoTracking()
                .Where(userTag => userTag.UserId == userId)
                .Select(useTag => useTag.TagId)
                .ToListAsync();

            var tags = await _repositoryTag
                .GetAll()
                .AsNoTracking()
                .Where(tag => tagIds.Contains(tag.Id))
                .ToListAsync();

            if (!tags.Any())
            {
                return null;
            }

            foreach (var tag in tags)
            {
                tagDtos.Add(new TagDto
                {
                   Id = tag.Id,
                   Name = tag.Name
                });
            }

            return tagDtos;
        }

        public async System.Threading.Tasks.Task UpdateTagAsync(TagDto tagDto)
        {
            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == tagDto.Id);
            if (tag is null)
            {
                return;
            }

            static bool ValidateToUpdate(Tag tag, TagDto tagDto)
            {
                bool updated = false;

                if (tag.Name != tagDto.Name)
                {
                    tag.Name = tagDto.Name;
                    updated = true;
                }

                return updated;
            }

            var result = ValidateToUpdate(tag, tagDto);
            if (result)
            {
                await _repositoryTag.SaveChangesAsync();
            }
        }
    }
}

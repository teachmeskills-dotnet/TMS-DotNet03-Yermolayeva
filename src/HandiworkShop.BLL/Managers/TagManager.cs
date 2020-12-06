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
        private readonly IRepository<OrderTag> _repositoryOrderTag;
        private readonly IRepository<Order> _repositoryOrder;

        public TagManager(
            IRepository<Tag> repositoryTag, 
            IRepository<UserTag> repositoryUserTag, 
            IRepository<OrderTag> repositoryOrderTag,
            IRepository<Order> repositoryOrder)
        {
            _repositoryTag = repositoryTag ?? throw new ArgumentNullException(nameof(repositoryTag));
            _repositoryUserTag = repositoryUserTag ?? throw new ArgumentNullException(nameof(repositoryUserTag));
            _repositoryOrderTag = repositoryOrderTag ?? throw new ArgumentNullException(nameof(repositoryOrderTag));
            _repositoryOrder = repositoryOrder ?? throw new ArgumentNullException(nameof(repositoryOrder));
        }

        public async System.Threading.Tasks.Task CreateAsync(TagDto tagDto)
        {
            tagDto = tagDto ?? throw new ArgumentNullException(nameof(tagDto));

            var tag = new Tag
            {
                Name = tagDto.Name
            };

            await _repositoryTag.CreateAsync(tag);
            await _repositoryTag.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == id);
            if (tag is null)
            {
                throw new KeyNotFoundException();
            }

            _repositoryTag.Delete(tag);
            await _repositoryTag.SaveChangesAsync();
        }

        public async Task<TagDto> GetTagAsync(int id)
        {
            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == id);
            if (tag is null)
            {
                throw new KeyNotFoundException();
            }

            var tagDto = new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
            return tagDto;
        }

        public async Task<IEnumerable<TagDto>> GetUserTagsAsync(string userId)
        {
            var tagDtos = new List<TagDto>();

            var tagIds = await _repositoryUserTag
                .GetAll()
                .AsNoTracking()
                .Where(userTag => userTag.UserId == userId)
                .Select(userTag => userTag.TagId)
                .ToListAsync();

            var tags = await _repositoryTag
                .GetAll()
                .AsNoTracking()
                .Where(tag => tagIds.Contains(tag.Id))
                .ToListAsync();

            if (tags.Any())
            {
                foreach (var tag in tags)
                {
                    tagDtos.Add(new TagDto
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }
            return tagDtos;
        }

        public async Task<IEnumerable<TagDto>> GetOrderTagsAsync(int orderId)
        {
            var tagDtos = new List<TagDto>();

            var tagIds = await _repositoryOrderTag
                .GetAll()
                .AsNoTracking()
                .Where(orderTag => orderTag.OrderId == orderId)
                .Select(orderTag => orderTag.TagId)
                .ToListAsync();

            var tags = await _repositoryTag
                .GetAll()
                .AsNoTracking()
                .Where(tag => tagIds.Contains(tag.Id))
                .ToListAsync();

            if (tags.Any())
            {
                foreach (var tag in tags)
                {
                    tagDtos.Add(new TagDto
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }        

            return tagDtos;
        }

        public async System.Threading.Tasks.Task UpdateTagAsync(TagDto tagDto)
        {
            tagDto = tagDto ?? throw new ArgumentNullException(nameof(tagDto));

            var tag = await _repositoryTag.GetEntityAsync(tag => tag.Id == tagDto.Id);
            if (tag is null)
            {
                throw new KeyNotFoundException();
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

        public async System.Threading.Tasks.Task UpdateUserTagsAsync(string userId, IList<int> tagIds)
        {
            var userTags = await _repositoryUserTag
                .GetAll()
                .AsNoTracking()
                .Where(userTag => userTag.UserId == userId)
                .ToListAsync();

            if (userTags.Any())
            {
                foreach (var tag in userTags)
                {
                    _repositoryUserTag.Delete(tag);
                }
            }

            if (tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    await _repositoryUserTag.CreateAsync(new UserTag()
                    {
                        TagId = tagId,
                        UserId = userId
                    });
                }
            }

            await _repositoryUserTag.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateOrderTagsAsync(int orderId, IList<int> tagIds, string userId)
        {
            var order = await _repositoryOrder.GetEntityAsync(order => order.Id == orderId && order.ClientId == userId);

            if (order is null)
            {
                throw new KeyNotFoundException();
            }

            var orderTags = await _repositoryOrderTag
               .GetAll()
               .AsNoTracking()
               .Where(orderTag => orderTag.OrderId == orderId)
               .ToListAsync();

            if (orderTags.Any())
            {
                foreach (var tag in orderTags)
                {
                    _repositoryOrderTag.Delete(tag);
                }
            }

            if (tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    await _repositoryOrderTag.CreateAsync(new OrderTag()
                    {
                        OrderId = orderId,
                        TagId = tagId
                    });
                }
            }

            await _repositoryOrderTag.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tagDtos = new List<TagDto>();
            var tags = await _repositoryTag
                .GetAll()
                .AsNoTracking()
                .ToListAsync();

            if (tags.Any())
            {
                foreach (var tag in tags)
                {
                    tagDtos.Add(new TagDto
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
            }

            return tagDtos;
        }
    }
}

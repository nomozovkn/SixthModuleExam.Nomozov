using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContactsManager.Dal.Entities;

namespace UserContactsManager.Bll.Repository.RepositoryService;

public interface IContactRepository
{
    Task<long> AddContactAsync(Contact contact);
    Task<Contact> GetContactByIdAsync(long contactId, long userId);
    Task<List<Contact>> GetAllContactsAsync(long userId);
    Task DeleteContactAsync(long contactId, long userId);
    Task UpdateContactAsync(Contact contact);
}
﻿using FluentValidation;
using UserContacts.Core.Errors;
using UserContactsManager.Bll.Dtos;
using UserContactsManager.Bll.Repository.RepositoryService;
using UserContactsManager.Dal.Entities;

namespace UserContacts.Bll.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepo;
    private readonly IValidator<ContactCreateDto> _createDtoValidator;
    private readonly IValidator<ContactDto> _updateDtoValidator;
    public ContactService(IContactRepository contactRepo, IValidator<ContactCreateDto> createDtoValidator, IValidator<ContactDto> updateDtoValidator)
    {
        _contactRepo = contactRepo;
        _createDtoValidator = createDtoValidator;
        _updateDtoValidator = updateDtoValidator;
    }
    private Contact Converter(ContactCreateDto contactCreateDto)
    {
        return new Contact
        {
            Address = contactCreateDto.Address,
            Email = contactCreateDto.Email,
            FirstName = contactCreateDto.FirstName,
            LastName = contactCreateDto.LastName,
            PhoneNumber = contactCreateDto.PhoneNumber,
        };
    }
    private ContactDto Converter(Contact contact)
    {
        return new ContactDto
        {
            Address = contact.Address,
            Email = contact.Email,
            FirstName = contact.FirstName,
            Id = contact.ContactId,
            PhoneNumber = contact.PhoneNumber,
            LastName = contact.LastName,


        };
    }
    public async Task<long> AddContactAsync(ContactCreateDto contactCreateDto, long userId)
    {
        var res = _createDtoValidator.Validate(contactCreateDto);
        if (!res.IsValid)
        {
            string errorMessages = string.Join("; ", res.Errors.Select(e => e.ErrorMessage));
            throw new NotAllowedException(errorMessages);
        }
        var contactEntity = Converter(contactCreateDto);
        contactEntity.UserId = userId;
        contactEntity.CreatedAt = DateTime.UtcNow;
        return await _contactRepo.AddContactAsync(contactEntity);
    }

    public async Task DeleteContactAsync(long contactId, long userId) => await _contactRepo.DeleteContactAsync(contactId, userId);

    public async Task<List<ContactDto>> GetAllContactsAsync(long userId)
    {
        var contacts = await _contactRepo.GetAllContactsAsync(userId);
        return contacts.Select(_ => Converter(_)).ToList();
    }

    public async Task<ContactDto> GetContactByIdAsync(long contactId, long userId) => Converter(await _contactRepo.GetContactByIdAsync(contactId, userId));
    public async Task UpdateContactAsync(ContactDto contactDto, long userId)
    {
        var res = _updateDtoValidator.Validate(contactDto);
        if (!res.IsValid)
        {
            string errorMessages = string.Join("; ", res.Errors.Select(e => e.ErrorMessage));
            throw new NotAllowedException(errorMessages);
        }
        var contact = await _contactRepo.GetContactByIdAsync(contactDto.Id, userId);
        contact.Email = contactDto.Email;
        contact.FirstName = contactDto.FirstName;
        contact.LastName = contactDto.LastName;
        contact.PhoneNumber = contactDto.PhoneNumber;
        contact.Address = contactDto.Address;
        await _contactRepo.UpdateContactAsync(contact);
    }
}

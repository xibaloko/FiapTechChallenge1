using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Tests
{
    //public class ContactsRegistrationTests
    //{
    //    [Fact]
    //    public void AddContact_ShouldAddNewContact()
    //    {
    //        // Arrange
    //        var registry = new ContactsRegistry();
    //        var contact = new Contact("John Doe", "123456789", "john.doe@example.com", "DDD1");

    //        // Act
    //        registry.AddContact(contact);

    //        // Assert
    //        Assert.Contains(contact, registry.Contacts);
    //    }

    //    // Other tests for input validation, etc.
    //}

    //public class ContactsQueryTests
    //{
    //    [Fact]
    //    public void QueryContactsByDDD_ShouldReturnContactsWithSameDDD()
    //    {
    //        // Arrange
    //        var query = new ContactsQuery();
    //        var registry = new ContactsRegistry();
    //        var contact1 = new Contact("John Doe", "123456789", "john.doe@example.com", "DDD1");
    //        var contact2 = new Contact("Jane Smith", "987654321", "jane.smith@example.com", "DDD2");
    //        registry.AddContact(contact1);
    //        registry.AddContact(contact2);

    //        // Act
    //        var filteredContacts = query.QueryContactsByDDD("DDD1");

    //        // Assert
    //        Assert.Contains(contact1, filteredContacts);
    //        Assert.DoesNotContain(contact2, filteredContacts);
    //    }

    //    // Other tests for empty query cases, etc.
    //}

    //public class ContactsUpdateDeletionTests
    //{
    //    [Fact]
    //    public void UpdateContact_ShouldUpdateExistingContact()
    //    {
    //        // Arrange
    //        var registry = new ContactsRegistry();
    //        var contact = new Contact("John Doe", "123456789", "john.doe@example.com", "DDD1");
    //        registry.AddContact(contact);
    //        var newContact = new Contact("Jane Smith", "987654321", "jane.smith@example.com", "DDD1");

    //        // Act
    //        registry.UpdateContact(contact, newContact);

    //        // Assert
    //        Assert.Contains(newContact, registry.Contacts);
    //        Assert.DoesNotContain(contact, registry.Contacts);
    //    }

    //    [Fact]
    //    public void DeleteContact_ShouldDeleteExistingContact()
    //    {
    //        // Arrange
    //        var registry = new ContactsRegistry();
    //        var contact = new Contact("John Doe", "123456789", "john.doe@example.com", "DDD1");
    //        registry.AddContact(contact);

    //        // Act
    //        registry.DeleteContact(contact);

    //        // Assert
    //        Assert.DoesNotContain(contact, registry.Contacts);
    //    }

    //    // Other tests for updating/deleting non-existing contacts, etc.
    //}
}
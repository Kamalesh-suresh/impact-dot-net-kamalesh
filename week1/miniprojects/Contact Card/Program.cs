
var contacts = new ContactCard[]
{
    new ContactCard { Name = "Alice",   Phone = "555-0101", Email = "alice@mail.com" },
    new ContactCard { Name = "Bob",     Phone = "555-0102", Email = "bob@mail.com" },
    new ContactCard { Name = "Charlie", Phone = "555-0103", Email = "charlie@mail.com" },
    new ContactCard { Name = "Diana",   Phone = "555-0104", Email = "diana@mail.com" },
    new ContactCard { Name = "Evan",    Phone = "555-0105", Email = "evan@mail.com" }
};


static void Search(ContactCard[]contacts , string query){
var found = Array.Find(contacts, c =>
    c.Name.Equals(query, StringComparison.OrdinalIgnoreCase));

// A struct is a VALUE type, so 'found' can never be null.
// A failed search returns default(ContactCard) — every field null.
// That's why we test a field instead of testing the struct itself.
if (found.Name == null)
    Console.WriteLine($"'{query}' — not found");
else
    Console.WriteLine($"'{query}' -> {found.Name} | {found.Phone} | {found.Email}");

}

Search(contacts, "alice");    // different case — still matches
Search(contacts, "BOB");      // different case — still matches
Search(contacts, "Zoe");


Console.ReadKey();
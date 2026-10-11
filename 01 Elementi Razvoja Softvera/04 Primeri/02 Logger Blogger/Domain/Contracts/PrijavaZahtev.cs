namespace Domain.Contracts;

/// <summary>Ulazni ugovor za prijavu korisnika.</summary>
public sealed record PrijavaZahtev(
    string KorisnickoIme,
    string Lozinka
);

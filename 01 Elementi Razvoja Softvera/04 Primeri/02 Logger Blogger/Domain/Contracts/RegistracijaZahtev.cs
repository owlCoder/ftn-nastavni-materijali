namespace Domain.Contracts;

/// <summary>Ulazni ugovor za registraciju korisnika.</summary>
public sealed record RegistracijaZahtev(
    string KorisnickoIme,
    string Lozinka,
    string ImePrezime
);

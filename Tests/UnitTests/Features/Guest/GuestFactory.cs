using ViaEventAssociation.Core.Domain.Agregates.Guests;

public class GuestFactory {
    private static readonly string _firstName = "John";
    private static readonly string _lastName = "Doe";
    private static readonly string _email = "JDO@via.dk";

    private Guest _guest;
    private GuestFactory() { }

    public static GuestFactory Init(string firstName, string lastName, string email) {
        var factory = new GuestFactory();
        factory._guest = Guest.Create(firstName, lastName, email).Payload;
        return factory;
    }

    public static GuestFactory InitWithDefaultsValues() {
        var factory = new GuestFactory();
        factory._guest = Guest.Create(_firstName, _lastName, _email).Payload;
        return factory;
    }

    public Guest Build() {
        return _guest;
    }
}
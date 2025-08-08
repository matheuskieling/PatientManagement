namespace FicharioDigital.Model;

public class ValidationResults {
    
    public ValidationField Name { get; set; } = new ValidationField();
    public ValidationField Cpf { get; set; } = new ValidationField();
    public ValidationField Rg { get; set; } = new ValidationField();
    public ValidationField FileNumber { get; set; } = new ValidationField();
    public ValidationField FileNumberEco { get; set; } = new ValidationField();
}
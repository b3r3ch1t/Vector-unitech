using FluentValidation;

namespace vector_unitech_core.ValueObjects
{
    public class Email : ValueObject<Email>
    {
        public const int EnderecoMaxLength = 254;
        public string Endereco { get; protected set; }
        public string Nome { get; protected set; }

        public Email( string endereco )
        {
            if ( string.IsNullOrWhiteSpace( endereco ) )
            {
                Endereco = string.Empty;
                return;
            }
            Endereco = endereco.ToLower();
        }

        public Email( string endereco, string nome )
        {
            if ( string.IsNullOrWhiteSpace( endereco ) || string.IsNullOrWhiteSpace( nome ) )
            {
                Endereco = string.Empty;
                Nome = string.Empty;
                return;
            }
            Endereco = endereco.ToLower();
            Nome = nome.ToUpper();
        }

        public override bool EhValido()
        {
            Validar();
            return ValidationResult.IsValid;

        }


        public override string ToString()
        {
            return Endereco.ToLower();
        }

        protected void Validar()
        {
            RuleFor( c => c.Endereco )
                .NotEmpty()
                .EmailAddress();

            ValidationResult = Validate( this );
        }
    }

}

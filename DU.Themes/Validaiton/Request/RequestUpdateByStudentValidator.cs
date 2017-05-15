using DU.Themes.Entities;
using FluentValidation.Results;

namespace DU.Themes.Validaiton.Request
{
    public class RequestUpdateByStudentValidator : RequestValidatorBase
    {
        public RequestUpdateByStudentValidator(DbContext context)
            : base(context)
        {
            this.Custom(x => this.NewOrNeedImporvements(x.Status));
        }

        private ValidationFailure NewOrNeedImporvements(RequestStatus status)
        {
            if (status == RequestStatus.New || status == RequestStatus.NeedImprovements)
            {
                return null;
            }

            return new ValidationFailure("Status", "Nav iespējams saglabāt jau atceltu pieteikumu");
        }
    }
}
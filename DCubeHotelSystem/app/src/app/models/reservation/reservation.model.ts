import { Customer } from '../customer.model';

/**	
 * Defines the model for Reservation Entity
 */
export interface Reservation {
	Id: number,
    IsAdvancePaid: boolean,
    AmountPaid: string,
    RDate: any,
    CustomerId: number,
    PaymentType: number,
    RoomTypeId: number,
    ReservationType: number,
    ReservationDetails: any[],
    SpecialRequest: string,
    CheckInDate: any,
    CheckOutDate: any,
    Adult: string;
    Children: string;
    NumberOfRoom: number,
    Status: string    
}

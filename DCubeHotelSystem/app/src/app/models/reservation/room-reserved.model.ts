/**	
 * Defines the model for Room reserverd entity
 */
export interface RoomReserverd {
	Id?: number;
	Adult?: string;
	Children?: string;	
	File?: string;
	IdentityFileName?: string;
	IdentityFileType?: string;	
	PhotoIdentity?: string;
	NumberofRoom?: string;
	ReservationId?: number;
	RoomTypeId?: string;
	RoomTypes?: string;
	ToCheckInDate?: any;
	ToCheckOutDate?: any;
}

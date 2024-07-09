export interface UserProfile {
    id: string;
    username: string;
    email: string;
    firstName: string | null;
    lastName: string | null;
    phone: string | null;
    profilePictureUrl: string | null;
}
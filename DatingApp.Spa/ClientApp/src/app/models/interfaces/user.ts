import { Photo } from "./photo";

export interface User {
    id: string;
    userName: string;
    knownAs: string;
    gender: string;
    created: Date;
    lastActive: Date
    age: number;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Array<Photo>;
    roles: string[];
}

import type { Artist } from "@/types/DTOs";
import { EntityService } from "./EntityService";
import type { ArtistRequest } from "@/types/DTOsRequest";

export class ArtistService extends EntityService<Artist, ArtistRequest> {
    constructor() {
        super('artists')
    }
}
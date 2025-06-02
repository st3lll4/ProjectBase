<script setup lang="ts">
import { ArtistService } from '@/services/ArtistService';
import type { Artist } from '@/types/DTOs';
import type { ArtistRequest } from '@/types/DTOsRequest';
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();
const artistAdminService = new ArtistService();

const error = ref<string | null>(null);
const success = ref<boolean>(false);
const loading = ref<boolean>(true);

const artist = ref<ArtistRequest>({
  id: '',
  stageName: '',
  birthDate: '',
  isSolo: true
});

const formatDateForInput = (isoDate: string): string => {
  if (!isoDate) return '';
  const date = new Date(isoDate);
  return date.toISOString().split('T')[0];
};

onMounted(async () => {
  const artistId = route.params.id as string;
  
  if (!artistId) {
    error.value = 'No artist ID provided';
    loading.value = false;
    return;
  }

  try {
    const result = await artistAdminService.getAsync(artistId);
    
    if (result.data) {
      const artistData = {
        ...result.data,
        birthDate: formatDateForInput(result.data.birthDate)
      };
      artist.value = artistData;
      loading.value = false;
    } else {
      error.value = result.errors?.[0] || 'Failed to load artist data';
      loading.value = false;
    }
  } catch (e) {
    error.value = 'An unexpected error occurred while loading artist data';
    loading.value = false;
    console.error(e);
  }
});

const updateArtist = async () => {
  error.value = null;
  success.value = false;

  try {
    const formattedArtist = {
      ...artist.value,
      birthDate: new Date(artist.value.birthDate).toISOString()
    };

    const result = await artistAdminService.updateAsync(formattedArtist as Artist);

    if (result.errors && result.errors.length !== 0) {
      error.value = result.errors?.[0] || 'Failed to update artist';
      return;
    }
    
    if (result.statusCode && result.statusCode <= 300) {
      success.value = true;
      router.push('/artists');
      
    } else {
      error.value = 'Failed to update artist';
    }
  } catch (e) {
    error.value = 'An unexpected error occurred';
    console.error(e);
  }
};
</script>

<template>
  <div class="card shadow-sm" style="border-color: #b39ddb; border-radius: 1rem;">
    <div class="card-body p-4">
      <h2 class="mb-4 text-center" style="color: #673ab7;">Edit artist</h2>
      
      <div v-if="loading" class="text-center my-5">
        <div class="spinner-border" role="status" style="color: #673ab7;">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
      
      <div v-else>
        <div v-if="error" class="alert alert-danger" role="alert">
          {{ error }}
        </div>
        
        <div v-if="success" class="alert" role="alert" style="background-color: #f3e5f5; color: #4a148c;">
          Artist updated successfully!
        </div>

        <form @submit.prevent="updateArtist">
          <input type="hidden" v-model="artist.id" />
          
        

          <div class="mb-3">
            <label for="stageName" class="form-label">Stage Name</label>
            <input
              v-model="artist.stageName"
              type="text"
              class="form-control"
              id="stageName"
              required
              style="border-color: #b39ddb;"
            />
          </div>

          <div class="mb-3">
            <label for="birthDate" class="form-label">Birth Date</label>
            <input
              v-model="artist.birthDate"
              type="date"
              class="form-control"
              id="birthDate"
              required
              style="border-color: #b39ddb;"
            />
          </div>

          <div class="mb-4 form-check">
            <input
              v-model="artist.isSolo"
              type="checkbox"
              class="form-check-input"
              id="isSolo"
            />
            <label class="form-check-label" for="isSolo">Solo Artist</label>
          </div>

          <div class="d-grid gap-2">
            <button
              type="submit"
              class="btn btn-lg"
              style="background-color: #9575cd; color: white;"
            >
              Update
            </button>
            
            <button
              type="button"
              class="btn btn-lg"
              style="background-color: #b39ddb; color: #4a148c;"
              @click="router.push('/artists')"
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<style scoped>
.form-control:focus {
  border-color: #b39ddb;
  box-shadow: 0 0 0 0.25rem rgba(103, 58, 183, 0.25);
}

.form-check-input:checked {
  background-color: #673ab7;
  border-color: #673ab7;
}

.btn:hover {
  background-color: #673ab7;
}
</style>
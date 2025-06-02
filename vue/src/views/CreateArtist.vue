<script setup lang="ts">
import { ref } from 'vue';
import { ArtistService } from '@/services/ArtistService';
import type { ArtistRequest } from '@/types/DTOsRequest';import router from '@/router';

const artistAdminService = new ArtistService();
const error = ref<string | null>(null);
const success = ref<boolean>(false);

const artist = ref<ArtistRequest>({
  stageName: '',
  birthDate: '',
  isSolo: true
});

const createArtist = async () => {
  error.value = null;
  success.value = false;

  try {
    const formattedArtist = {
      ...artist.value,
      birthDate: new Date(artist.value.birthDate).toISOString()
    };

    const result = await artistAdminService.addAsync(formattedArtist);

    if (result.errors && result.errors.length !== 0) {
      error.value = result.errors?.[0] || 'Failed to create artist';
      return
    }
    
    if (result.data) {
      success.value = true;
      artist.value = {
        stageName: '',
        birthDate: '',
        isSolo: true
      };
      
      setTimeout(() => {
        router.push('/artists');
      }, 1000);
    } else {
      error.value = result.errors?.[0] || 'Failed to create artist';
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
      <h2 class="mb-4 text-center" style="color: #673ab7;">New artist</h2>
      
      <div v-if="error" class="alert alert-danger" role="alert">
        {{ error }}
      </div>
      
      <div v-if="success" class="alert" role="alert" style="background-color: #b39ddb; color: #673ab7;">
        Artist created successfully!
      </div>

      <form @submit.prevent="createArtist">
        
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

        <div class="d-grid">
          <button
            type="submit"
            class="btn btn-lg"
            style="background-color: #b39ddb; color: white;"
          >
            Create Artist
          </button>
        </div>
      </form>
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
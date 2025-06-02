<script setup lang="ts">
import { ArtistService } from '@/services/ArtistService';
import type { Artist } from '@/types/DTOs';
import type { ArtistRequest } from '@/types/DTOsRequest';
import { ref, onMounted } from 'vue';

const artistService = new ArtistService();
const artistAdminService = new ArtistService();
const artists = ref<Artist[]>([]);
const loading = ref<boolean>(true);
const error = ref<string | null>(null);
const successMessage = ref<string | null>(null);

const showDeleteModal = ref(false);
const artistToDelete = ref<Artist | null>(null);

const loadArtists = async () => {
  try {
    const result = await artistService.getAllAsync();
    artists.value = result.data || [];
    loading.value = false;
  } catch (e) {
    error.value = 'Failed to load artists';
    loading.value = false;
    console.error(e);
  }
};

onMounted(loadArtists);

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString();
};

const confirmDelete = (artist: Artist) => {
  artistToDelete.value = artist;
  showDeleteModal.value = true;
};

const cancelDelete = () => {
  artistToDelete.value = null;
  showDeleteModal.value = false;
};

const deleteArtist = async () => {
  if (!artistToDelete.value) return;
  
  try {
    const result = await artistAdminService.deleteAsync(artistToDelete.value.id);
    
    if (result.statusCode && result.statusCode < 400) {
      successMessage.value = `Artist "${artistToDelete.value.stageName}" has been deleted successfully.`;
      
      await loadArtists();
      
      setTimeout(() => {
        successMessage.value = null;
      }, 3000);
    } else {
      error.value = result.errors?.[0] || 'Failed to delete artist';
      setTimeout(() => {
        error.value = null;
      }, 3000);
    }
  } catch (e) {
    error.value = 'An unexpected error occurred during deletion';
    console.error(e);
  } finally {
    cancelDelete();
  }
};
</script>

<template>
  <div class="card shadow-sm" style="border-color: #b39ddb; border-radius: 1rem;">
    <div class="card-body p-4">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 style="color: #673ab7;">Artists</h2>
        <router-link to="/artist/create" class="btn" style="background-color: #9575cd; color: white;">
          Create New Artist
        </router-link>
      </div>
      
      <div v-if="error" class="alert alert-danger" role="alert">
        {{ error }}
      </div>
      
      <div v-if="successMessage" class="alert" role="alert" style="background-color: #f3e5f5; color: #673ab7;">
        {{ successMessage }}
      </div>
      
      <div v-if="loading" class="text-center my-5">
        <div class="spinner-border" role="status" style="color: #673ab7;">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
      
      <div v-else-if="artists.length === 0" class="text-center my-5 text-muted">
        <p>No artists found</p>
      </div>
      
      <div v-else class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>Stage Name</th>
              <th>Birth Date</th>
              <th>Type</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="artist in artists" :key="artist.id">
              <td>{{ artist.stageName }}</td>
              <td>{{ formatDate(artist.birthDate) }}</td>
              <td>{{ artist.isSolo ? 'Solo Artist' : 'Group Member' }}</td>
              <td>
                <div class="d-flex gap-2">
                  <router-link :to="`/artist/edit/${artist.id}`" class="btn btn-sm" 
                    style="background-color: #b39ddb; color: #4a148c;">
                    Edit
                  </router-link>
                  <button @click="confirmDelete(artist)" class="btn btn-sm"
                    style="background-color: #e1bee7; color: #4a148c;">
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  
  <div class="modal fade" :class="{ 'show d-block': showDeleteModal }" tabindex="-1" 
    style="background-color: rgba(0,0,0,0.5);" v-if="showDeleteModal">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header" style="background-color: #f3e5f5; color: #673ab7;">
          <h5 class="modal-title">Confirm Deletion</h5>
          <button type="button" class="btn-close" @click="cancelDelete"></button>
        </div>
        <div class="modal-body">
          <p>Are you sure you want to delete the artist <strong>{{ artistToDelete?.stageName }}</strong>?</p>
          <p class="text-danger">This action cannot be undone.</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn" style="background-color: #b39ddb; color: #4a148c;" @click="cancelDelete">
            Cancel
          </button>
          <button type="button" class="btn" style="background-color: #9575cd; color: white;" @click="deleteArtist">
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.table {
  --bs-table-hover-bg: #f3e5f5;
}

.modal.fade {
  transition: opacity 0.15s linear;
}

.modal {
  background-color: rgba(103, 58, 183, 0.5);
}

.btn:hover {
  opacity: 0.9;
}
</style>
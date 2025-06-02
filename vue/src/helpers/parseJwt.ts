export function parseJwt(token?: string): { roles?: string[], name?: string } {
    if (!token) return { roles: undefined, name: undefined };
    
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
  
      const parsed = JSON.parse(jsonPayload);
      
      return {
        roles: parsed.role || [],
        name: parsed.username
      };
    } catch (e) {
      console.error('Error parsing JWT token', e);
      return { roles: undefined, name: undefined };
    }
  }
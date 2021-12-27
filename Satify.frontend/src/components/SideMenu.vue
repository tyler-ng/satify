<template>
  <div class="side-menu-container">
    <router-link to="/">
      <img
        id="imgLogo"
        src="../assets/images/satify.png"
        alt="Tofu and rolls logo"
      />
    </router-link>
    <h1>Management Portal</h1>
    <satify-button
      id="menuInventory"
      is-full-width
      @button:click="goToRoute('/inventory')"
    >
      Inventory
    </satify-button>
    <satify-button
      id="menuCustomers"
      is-full-width
      @button:click="goToRoute('/customers')"
    >
      Manage Customers
    </satify-button>
    <satify-button
      id="menuInvoice"
      is-full-width
      @button:click="goToRoute('/invoice/new')"
    >
      New Invoice
    </satify-button>
    <satify-button
      id="menuOrders"
      is-full-width
      @button:click="goToRoute('/orders')"
    >
      Orders
    </satify-button>
    <satify-button
      id="menuSalespersons"
      is-full-width
      @button:click="goToRoute('/salespersons')"
    >
      Manage Salespersons
    </satify-button>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SatifyButton from "@/components/SatifyButton.vue";

@Component({
  name: "SideMenu",
  components: { SatifyButton },
})
export default class SideMenu extends Vue {
  goToRoute(route: string) {
    this.$router.push(route);
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.side-menu-container {
  background-color: #fcfcfc;
  height: 100vh;
  width: $menu-width;
  display: flex;
  flex-direction: column;
  padding: 0.8rem;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.12), 0 1px 1px rgba(0, 0, 0, 0.24);
  box-sizing: border-box;
}

#imgLogo {
  width: 100%;
}

h1 {
  font-size: 1.2rem;
  margin-top: 1.5rem;
  margin-bottom: 1.5rem;
}
</style>

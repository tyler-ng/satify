<template>
  <div>
    <h1 id="customersTitle">Manage Customers</h1>
    <hr />
    <div class="customer-actions">
      <satify-button @button:click="showNewCustomerModal">
        Add Customer
      </satify-button>
    </div>
    <table id="customers" class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>Province</th>
        <th>Postal Code</th>
        <th>Since</th>
        <th>Details</th>
      </tr>
      <tr v-for="customer in customers" :key="customer.id">
        <td>
          {{ customer.firstName + " " + customer.lastName }}
        </td>
        <td>
          {{ customer.primaryAddress.addressLine1 }}
        </td>
        <td>
          {{ customer.primaryAddress.province }}
        </td>
        <td>
          {{ customer.primaryAddress.postalCode }}
        </td>
        <td>
          {{ customer.createdOn | humanizeDate }}
        </td>
        <td>
          <div
            class="lni-radio-button customer-detail"
            @click="goToDetailCustomer(customer)"
          ></div>
        </td>
      </tr>
    </table>
    <new-customer-modal
      v-if="isCustomerModalVisible"
      @save:customer="saveNewCustomer"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SatifyButton from "@/components/SatifyButton.vue";
import { ICustomer } from "@/types/Customer";
import CustomerService from "@/services/customer-service";
import NewCustomerModal from "@/components/modals/NewCustomerModal.vue";
import { ISalesOrder } from "@/types/Order";
import OrderService from "@/services/order-service";

const customerService = new CustomerService();

@Component({
  name: "Customers",
  components: { NewCustomerModal, SatifyButton },
})
export default class Customers extends Vue {
  isCustomerModalVisible: boolean = false;

  customers: ICustomer[] = [];
  customer: ICustomer = {
    id: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    firstName: "",
    lastName: "",
  };

  showNewCustomerModal() {
    this.isCustomerModalVisible = true;
  }

  async saveNewCustomer(newCustomer: ICustomer) {
    await customerService.addCustomer(newCustomer);
    this.isCustomerModalVisible = false;
    await this.initialize();
  }

  closeModals() {
    this.isCustomerModalVisible = false;
  }

  goToDetailCustomer(customer: ICustomer) {
    this.$router.push({
      name: "customer-details",
      params: { customerId: `${customer.id}` },
    });
  }

  async initialize() {
    this.customers = await customerService.getCustomers();
  }

  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.customer-actions {
  display: flex;
  margin-bottom: 0.8rem;

  div {
    margin-right: 0.8rem;
  }
}

.customer-edit {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $satify-green;
}

.customer-detail {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $satify-blue;
}

.customer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $satify-red;
}
</style>

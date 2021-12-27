<template>
  <div>
    <h1 id="customerDetailsTitle">Customer Details</h1>
    <hr />
    <div class="customer-details-action">
      <satify-button @button:click="editCustomerDetails">
        {{ editButtonContext }}
      </satify-button>
    </div>
    <div class="customer-details">
      <ul class="customerDetails">
        <li>
          <label for="firstName">First Name</label>
          <input
            type="text"
            id="firstName"
            :disabled="!isEditButtonVisible"
            v-model="customer.firstName"
          />
        </li>
        <li>
          <label for="lastName">Last Name</label>
          <input
            type="text"
            id="lastName"
            :disabled="!isEditButtonVisible"
            v-model="customer.lastName"
          />
        </li>
        <li>
          <label for="address1">Address Line 1</label>
          <input
            type="text"
            id="address1"
            :disabled="!isEditButtonVisible"
            v-model="customer.primaryAddress.addressLine1"
          />
        </li>
        <li>
          <label for="city">City</label>
          <input
            type="text"
            id="city"
            :disabled="!isEditButtonVisible"
            v-model="customer.primaryAddress.city"
          />
        </li>
        <li>
          <label for="postalCode">Postal Code</label>
          <input
            type="text"
            id="postalCode"
            :disabled="!isEditButtonVisible"
            v-model="customer.primaryAddress.postalCode"
          />
        </li>
        <li>
          <label for="province">Province</label>
          <input
            type="text"
            id="province"
            :disabled="!isEditButtonVisible"
            v-model="customer.primaryAddress.province"
          />
        </li>
        <li>
          <label for="country">Country</label>
          <input
            type="text"
            id="country"
            :disabled="!isEditButtonVisible"
            v-model="customer.primaryAddress.country"
          />
        </li>
      </ul>
    </div>
    <div id="customerSince">
      <p>
        <strong>Customer since</strong>: {{ customer.createdOn | humanizeDate }}
      </p>
    </div>
    <h2 id="customer-order-history">Orders History</h2>
    <hr />
    <table id="order-history" class="table">
      <thead>
        <tr>
          <th>Id</th>
          <th>Total</th>
          <th>Date</th>
          <th>Status</th>
        </tr>
      </thead>
      <tr v-for="order in saleOrders" :key="order.id">
        <td>
          {{ order.id }}
        </td>
        <td>
          {{ getTotal(order) | money }}
        </td>
        <td>
          {{ order.createdOn | humanizeDate }}
        </td>
        <td :class="{ green: order.isPaid }">
          {{ getStatus(order.isPaid) }}
        </td>
      </tr>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { ICustomer } from "@/types/Customer";
import CustomerService from "@/services/customer-service";
import SatifyButton from "@/components/SatifyButton.vue";
import { ISalesOrder } from "@/types/Order";
import OrderService from "@/services/order-service";

const customerService = new CustomerService();
const orderService = new OrderService();

@Component({
  name: "CustomerDetails",
  components: { SatifyButton },
})
export default class CustomerDetails extends Vue {
  customer: ICustomer = {
    id: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    firstName: "",
    lastName: "",
    primaryAddress: {},
  };
  saleOrders: ISalesOrder[] = [];

  isEditButtonVisible: boolean = false;
  editButtonContext: string = "Edit";

  async editCustomerDetails() {
    if (this.editButtonContext === "Edit") {
      this.isEditButtonVisible = true;
      this.editButtonContext = "Done";
    } else {
      await customerService.editCustomer(this.customer);
      this.isEditButtonVisible = false;
      this.editButtonContext = "Edit";
    }
  }

  getTotal(order: ISalesOrder) {
    return order.salesOrderItems.reduce(
      (a, b) => a + b["product"]!["price"] * b["quantity"],
      0
    );
  }

  getStatus(isPaid: boolean) {
    return isPaid ? "Paid in Full" : "Unpaid";
  }

  async initialize() {
    const customerId = Number(this.$route.params.customerId);
    this.customer = await customerService.getCustomerById(customerId);
    this.saleOrders = await orderService.getOrdersByCustomer(customerId);
  }

  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
.customer-details-action {
  text-align: right;
}

.customerDetails {
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  padding: 0;
  margin: 0;

  input {
    width: 80%;
    height: 1.8rem;
    margin: 0.8rem 2rem 0.8rem 0.8rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  label {
    font-weight: bold;
    margin: 0.8rem;
    display: block;
  }
}

#customerSince {
  padding: 0.8rem;
  margin-bottom: 2rem;
}

.customer-details-action {
  margin-bottom: 2rem;
}

.green {
  font-weight: bold;
  color: $satify-green;
}
</style>
